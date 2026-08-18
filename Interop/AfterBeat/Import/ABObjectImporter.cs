using System;
using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Interfaces.Keyframes;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat.Import
{
    // Reads Afterbeat's objects into this format's. Four conversions in here are the ones that go
    // wrong invisibly, so they are named up front:
    //
    // 1. A keyframe's time is relative to its object's start in BOTH formats, so it crosses
    //    unchanged. It looks like the sort of thing that needs converting, and converting it is
    //    what produces objects that spawn correctly and then never move.
    // 2. Rotation is degrees and RELATIVE per keyframe there, radians and ABSOLUTE here. Both
    //    halves are done by ABValueMap; either one missed produces a level that spins.
    // 3. Depth is absolute there and Layer is parent-relative here, so a child's layer is its own
    //    effective layer minus its parent's. Reading obj.d straight into Layer draws a whole
    //    hierarchy at the wrong depth as soon as anything is parented.
    // 4. Afterbeat's "scale" is a width/height in world units, which is this format's SIZE, not its
    //    Scale - except on TEXT, where it is the only thing sizing the glyphs and therefore crosses
    //    as Scale instead, the Size being the block the glyphs lay out in. See ApplyTextSize.
    //
    // Import is TWO passes over the object list on purpose - see ABImportContext.

    /// <summary> One Afterbeat object into one <see cref="RectObject"/>. </summary>
    public static class ABObjectImporter
    {
        /// <summary> Mints ids and effective layers for every source object, then fills them in.
        /// Both first, because a child may be written before its parent - and because two of the
        /// four layer modes cannot answer for one object without having seen the whole list. </summary>
        public static void ImportAll(IReadOnlyList<VgdObject> sources, ABImportContext context,
            string pathPrefix)
        {
            if (sources == null || context?.Scope?.Objects == null) return;

            var layers = ABLayerMap.Resolve(sources, context.Options, context.Report, pathPrefix);
            context.RegisterContentLayers(layers.Lowest, layers.Highest);

            for (var i = 0; i < sources.Count; i++)
            {
                var source = sources[i];
                if (source == null) continue;

                context.Mint(source.Id);
                context.SetEffectiveLayer(source.Id, layers.Layers[i]);
            }

            var byId = IndexById(sources);
            ResolveScaleTargets(sources, byId, context, pathPrefix);
            ResolveAxisSwaps(sources, byId, context);

            for (var i = 0; i < sources.Count; i++)
            {
                var source = sources[i];
                if (source == null) continue;

                var path = $"{pathPrefix}[{i}]";
                var imported = Import(source, context, layers.Layers[i], path);
                if (imported == null) continue;

                ApplyParentOrigin(source, imported, byId);
                ReportShear(source, byId, context.Report, path);
                context.Scope.Objects[imported.ObjectId] = imported;
            }
        }

        // A QUARTER TURN UNDER A SQUASHED PARENT IS NOT SHEAR, and reading it as one is how the
        // worst objects in a converted level got their worst numbers. Afterbeat composes matrices,
        // so a child's linear map is R(rp)·S(sp)·R(rc)·S(sc), and S does not commute with R unless
        // the scale is uniform. At exactly a quarter turn, though, it ALMOST does:
        //
        //     S(x, y)·R(90) == R(90)·S(y, x)
        //
        // The product is still a plain rotation and scale - the parent's two scale components have
        // simply traded places. Nothing is skewed and nothing is unrepresentable; there is an exact
        // answer, and composing in this format's own order (rotate by the sum, scale by the product)
        // reaches the wrong one. On a parent squashed 35:1 that is not a subtle error: measured on a
        // real level, one object came out 1710 by 3.18 where it should have been 47.7 by 114.
        //
        // So the child's own scale is multiplied by the ratio that undoes the trade. Position needs
        // nothing - a parent's scale reaches a child's offset identically in both models - and it
        // must NOT be folded into the position for the same reason.
        //
        // Exact when the parent's scale is constant and one link in the chain turns; a second turned
        // link below it composes against the accumulated scale rather than the parent's own, which
        // this per-hop rule approximates rather than solves. Measured over two real levels, the rule
        // covers 96 objects, and they are the anisotropic ones - three quarters of them sit above
        // 1.5:1, where an axis trade is the difference between a shape and a streak.
        private static void ResolveAxisSwaps(IReadOnlyList<VgdObject> sources,
            IReadOnlyDictionary<string, VgdObject> byId, ABImportContext context)
        {
            foreach (var source in sources)
            {
                if (source == null || string.IsNullOrEmpty(source.ParentId)) continue;
                if (!InheritsScale(source)) continue;
                if (!byId.TryGetValue(source.ParentId, out var parent) || parent == null) continue;
                if (!IsQuarterTurned(source)) continue;

                if (!TryGetConstantScale(parent, out var x, out var y)) continue;
                if (Math.Abs(x) < MinCompensableScale || Math.Abs(y) < MinCompensableScale) continue;
                if (Math.Abs(x - y) <= ShearEpsilon) continue;

                if (!string.IsNullOrEmpty(source.Id))
                    context.AxisSwaps[source.Id] = (y / x, x / y);
            }
        }

        /// <summary> Whether an object's own rotation is a constant quarter or three-quarter turn -
        /// the two angles at which a non-uniform parent scale trades axes rather than skewing. </summary>
        public static bool IsQuarterTurned(VgdObject source)
        {
            if (!TryGetConstantRotation(source, out var degrees)) return false;

            var wrapped = ((degrees % 360f) + 360f) % 360f;
            return Math.Abs(wrapped - 90f) <= QuarterTurnEpsilon
                   || Math.Abs(wrapped - 270f) <= QuarterTurnEpsilon;
        }

        /// <summary> How far off a quarter turn still counts as one. Anything further is a genuine
        /// skew, and pretending otherwise would trade a visible error for a different one. </summary>
        public const float QuarterTurnEpsilon = 0.5f;

        // Afterbeat's rotation keyframes are DELTAS, so a constant rotation is one whose deltas
        // after the first are all zero - not one with a single keyframe.
        private static bool TryGetConstantRotation(VgdObject source, out float degrees)
        {
            degrees = 0f;
            var keyframes = source.Rotate?.Keyframes;
            if (keyframes == null || keyframes.Count == 0) return false;

            for (var i = 0; i < keyframes.Count; i++)
            {
                var value = keyframes[i]?.GetValue(0) ?? 0f;
                if (i == 0) degrees = value;
                else if (Math.Abs(value) > ShearEpsilon) return false;
            }
            return true;
        }

        private static bool TryGetConstantScale(VgdObject source, out float x, out float y)
        {
            x = 1f;
            y = 1f;

            var keyframes = source.Scale?.Keyframes;
            if (keyframes == null || keyframes.Count == 0) return false;

            x = keyframes[0]?.GetValue(0) ?? 1f;
            y = keyframes[0]?.GetValue(1) ?? 1f;

            foreach (var key in keyframes)
            {
                if (Math.Abs((key?.GetValue(0) ?? 0f) - x) > ShearEpsilon) return false;
                if (Math.Abs((key?.GetValue(1) ?? 0f) - y) > ShearEpsilon) return false;
            }
            return true;
        }

        // WHAT IS LEFT AFTER THE QUARTER TURNS IS GENUINE SHEAR. Afterbeat parents with
        // plain Unity transforms, and a non-uniform scale above a rotation SHEARS what is under it -
        // a square under a parent scaled (3, 1) and turned 30 degrees comes out a parallelogram.
        // This format composes a rotation and a per-axis scale without a matrix between them, so it
        // has no shear to give: the same object comes out a rotated rectangle.
        //
        // Nothing here can fix that, and nothing here should pretend to - what it can do is say
        // which objects it happens to, since they are the ones that will look subtly wrong while
        // every number in them is right. Reported only when all three conditions actually meet, so
        // an ordinary level says nothing.
        //
        // The quarter turns are deliberately EXCLUDED. They used to be reported here, which was a
        // wrong diagnosis twice over: it called an exactly representable composition unrepresentable,
        // and it buried the objects that really are skewed among the ones that are now correct.
        private static void ReportShear(VgdObject source, IReadOnlyDictionary<string, VgdObject> byId,
            InteropReport report, string path)
        {
            if (string.IsNullOrEmpty(source.ParentId) || !InheritsScale(source)) return;
            if (!byId.TryGetValue(source.ParentId, out var parent) || parent == null) return;
            if (!IsNonUniformlyScaled(parent) || !IsRotated(source)) return;
            if (IsQuarterTurned(source)) return;

            report.Approximated("parent_scale_shear",
                "Afterbeat skews a rotated object sitting under a non-uniformly scaled parent; this format rotates and scales without skewing, so those objects keep their shape. The wider apart the parent's two scales are, the more it shows.",
                path);
        }

        private static bool IsNonUniformlyScaled(VgdObject source)
        {
            var keyframes = source.Scale?.Keyframes;
            if (keyframes == null) return false;

            foreach (var key in keyframes)
            {
                if (key == null) continue;
                if (Math.Abs(key.GetValue(0) - key.GetValue(1)) > ShearEpsilon) return true;
            }
            return false;
        }

        private static bool IsRotated(VgdObject source)
        {
            var keyframes = source.Rotate?.Keyframes;
            if (keyframes == null) return false;

            foreach (var key in keyframes)
            {
                if (key == null) continue;
                if (Math.Abs(key.GetValue(0)) > ShearEpsilon) return true;
            }
            return false;
        }

        /// <summary> Below this a scale counts as uniform and a rotation as none - a hundredth of a
        /// degree, or of a world unit, shears nothing anybody can see. </summary>
        public const float ShearEpsilon = 0.01f;

        private static Dictionary<string, VgdObject> IndexById(IReadOnlyList<VgdObject> sources)
        {
            var byId = new Dictionary<string, VgdObject>(sources.Count);
            foreach (var source in sources)
                if (source != null && !string.IsNullOrEmpty(source.Id))
                    byId[source.Id] = source;
            return byId;
        }

        #region Scale inheritance

        // The whole of ABImportContext.ScaleTarget's header applies here; this is the pass that
        // fills it. Two things are decided per object:
        //
        //   which FIELD its own scale track goes into, read off its CHILDREN's masks - the mask is
        //   a property of the child in the source format, so an object with no children has no
        //   opinion and keeps the plain Size;
        //
        //   and, when its children disagree with each other, which of them has to be COMPENSATED,
        //   since one field cannot serve both. The majority side keeps the free, exact, animation-
        //   proof mapping and the minority is divided or multiplied by the parent's own scale.
        //   That bake is exact only while the parent's scale is CONSTANT, which is what 40 of the
        //   44 mixed parents in the levels this was measured against are; the rest are reported,
        //   because there is no per-frame compensation this format can hold without resampling
        //   both objects' tracks onto each other.
        private static void ResolveScaleTargets(IReadOnlyList<VgdObject> sources,
            IReadOnlyDictionary<string, VgdObject> byId, ABImportContext context, string path)
        {
            var inheriting = new Dictionary<string, int>();
            var total = new Dictionary<string, int>();

            foreach (var source in sources)
            {
                if (source?.ParentId == null || string.IsNullOrEmpty(source.ParentId)) continue;
                if (!byId.ContainsKey(source.ParentId)) continue;

                total.TryGetValue(source.ParentId, out var count);
                total[source.ParentId] = count + 1;

                if (!InheritsScale(source)) continue;
                inheriting.TryGetValue(source.ParentId, out var inherited);
                inheriting[source.ParentId] = inherited + 1;
            }

            foreach (var pair in total)
            {
                inheriting.TryGetValue(pair.Key, out var inherited);
                var target = inherited * 2 >= pair.Value
                    ? ABImportContext.ScaleTarget.Scale
                    : ABImportContext.ScaleTarget.Size;

                context.ScaleTargets[pair.Key] = target;

                var minority = target == ABImportContext.ScaleTarget.Scale
                    ? pair.Value - inherited
                    : inherited;
                if (minority > 0)
                    RecordCompensation(pair.Key, byId, sources, target, context, path);
            }
        }

        private static void RecordCompensation(string parentId,
            IReadOnlyDictionary<string, VgdObject> byId, IReadOnlyList<VgdObject> sources,
            ABImportContext.ScaleTarget target, ABImportContext context, string path)
        {
            if (!byId.TryGetValue(parentId, out var parent)) return;

            var scaleTrack = parent.Scale;
            var animated = scaleTrack?.Keyframes is { Count: > 1 };

            if (animated)
            {
                context.Report.Approximated("scale_inheritance_mixed_animated",
                    "Some objects have children that disagree about inheriting their scale, and their own scale is animated; this format cannot express both at once, so the disagreeing children follow the majority.",
                    path);
                return;
            }

            var first = scaleTrack?.Keyframes is { Count: > 0 } ? scaleTrack.Keyframes[0] : null;
            var x = first?.GetValue(0) ?? 1f;
            var y = first?.GetValue(1) ?? 1f;
            if (Math.Abs(x) < MinCompensableScale || Math.Abs(y) < MinCompensableScale) return;

            // Divide when the parent's scale now reaches the child and should not; multiply when it
            // no longer reaches one that expected it.
            var factorX = target == ABImportContext.ScaleTarget.Scale ? 1f / x : x;
            var factorY = target == ABImportContext.ScaleTarget.Scale ? 1f / y : y;
            if (Math.Abs(factorX - 1f) < float.Epsilon && Math.Abs(factorY - 1f) < float.Epsilon)
                return;

            foreach (var child in sources)
            {
                if (child?.ParentId != parentId) continue;
                var inherits = InheritsScale(child);
                var isMinority = target == ABImportContext.ScaleTarget.Scale ? !inherits : inherits;
                if (isMinority && !string.IsNullOrEmpty(child.Id))
                    context.ScaleCompensations[child.Id] = (factorX, factorY);
            }
        }

        /// <summary> Below this a parent's scale cannot be divided out of a child without
        /// producing an infinity - a zero-scaled parent draws nothing anyway. </summary>
        public const float MinCompensableScale = 1e-4f;

        /// <summary> Positional meaning of the three characters of p_t. </summary>
        public static class ParentTypeIndex
        {
            public const int Position = 0;
            public const int Scale = 1;
            public const int Rotation = 2;
        }

        /// <summary> Whether one source object inherits its parent's scale. The mask lives on the
        /// CHILD, and a document that wrote none means the format's own default - which does not
        /// inherit it. </summary>
        public static bool InheritsScale(VgdObject source) => HasParentBit(source, ParentTypeIndex.Scale);

        private static bool HasParentBit(VgdObject source, int index)
        {
            var mask = source?.ParentType;
            if (string.IsNullOrEmpty(mask)) mask = VgdObject.DefaultParentType;
            return index < mask.Length && mask[index] == '1';
        }

        #endregion

        /// <summary> One object, at an effective layer <see cref="ABLayerMap"/> already
        /// resolved for the whole list. Returns null only when the source is null. </summary>
        public static RectObject Import(VgdObject source, ABImportContext context,
            int effectiveLayer, string path)
        {
            if (source == null) return null;

            var report = context.Report;
            var framerate = context.Options.Framerate;

            var target = CreateTarget(source, context, path);
            target.ObjectId = context.Mint(source.Id);
            target.Name = context.Options.KeepObjectNames
                ? (string.IsNullOrEmpty(source.Name) ? source.Id ?? string.Empty : source.Name)
                : string.Empty;
            target.Active = true;
            target.Span = ABTimeMap.ResolveSpan(source, framerate, report, path);

            // A mask of "000" inherits nothing at all, which this format cannot say of a child -
            // but CAN say of a root, exactly and with nothing baked. So such an object is imported
            // as one, keeping its own keyframes untouched. It only loses the parent's own lifetime
            // bounding it, which is bookkeeping rather than motion.
            target.ParentObjectId = InheritsNothing(source)
                ? ObjectId.Null
                : context.ResolveParent(source.ParentId, path);

            ApplyLayer(source, target, context, effectiveLayer);
            ApplyPivot(source, target, report, path);
            ReportParenting(source, report, path);

            ImportPositions(source, target, framerate, context, path);
            ImportScales(source, target, framerate, context, path);
            ApplyTextSize(target, report, path);
            ImportRotations(source, target, framerate, report, path);
            ImportColors(source, target, context, path);

            return target;
        }

        /// <summary> Whether a source object's mask inherits none of the three channels, which is
        /// the one mask this format has an exact answer for. </summary>
        public static bool InheritsNothing(VgdObject source)
            => !string.IsNullOrEmpty(source?.ParentId)
               && !HasParentBit(source, ParentTypeIndex.Position)
               && !HasParentBit(source, ParentTypeIndex.Scale)
               && !HasParentBit(source, ParentTypeIndex.Rotation);

        #region Type

        // Hit and No Hit are the same object with and without a collider, which is exactly how this
        // format expresses the distinction. Empty carries no geometry at all and becomes the base
        // type - it is a transform other objects hang off, and giving it a shape would draw
        // something the source level never drew.
        private static RectObject CreateTarget(VgdObject source, ABImportContext context, string path)
        {
            if (ABShapeMap.IsText(source.Shape))
                return CreateText(source, context, path);

            if (IsEmpty(source.ObjectType))
                return new RectObject();

            var shapeId = ABShapeMap.Import(source.Shape, source.ShapeOption,
                context.Shapes, context.Report, path, source);

            return new ShapeObject
            {
                ShapeId = shapeId,
                ColliderId = IsHit(source.ObjectType, context.Report, path) ? shapeId : ShapeId.Null,
                ShaderType = ShaderType.Auto,
            };
        }

        // An unknown type is read as Hit rather than as No Hit, which is the direction that fails
        // safely: an object that should not have hurt the player is a level that is too hard to
        // beat, while a missing collider is a level that cannot be lost - and the second one is
        // silent, since nothing on screen looks any different.
        private static bool IsHit(int objectType, InteropReport report, string path)
        {
            switch ((ABObjectType)objectType)
            {
                case ABObjectType.Normal:
                case ABObjectType.Hit:
                    return true;
                case ABObjectType.Helper:
                case ABObjectType.Decoration:
                case ABObjectType.NoHit:
                    return false;
                case ABObjectType.Particles:
                    report.Approximated("object_type_particles",
                        "Afterbeat particle emitters have no equivalent here; those objects were imported as their own shape, drawn once and never hitting the player, and emit nothing.",
                        path);
                    return false;
                default:
                    report.Approximated("object_type_unknown",
                        $"Object type {objectType} is not one this converter knows; those objects were imported as ordinary hitting objects.",
                        path);
                    return true;
            }
        }

        private static bool IsEmpty(int objectType)
            => (ABObjectType)objectType is ABObjectType.Empty or ABObjectType.AlphaEmpty;

        // Afterbeat text has NO bounds of its own - it lays out from its origin and runs as far as
        // it needs to. This format lays text out inside the object's Size, so an imported text has
        // to be given one, and there is nothing in the source document to compute it from.
        //
        // So it is ESTIMATED, on the crudest rule that cannot clip: one character wide per
        // character of the longest line, one line tall per line. That over-reserves for most
        // typefaces (glyphs are narrower than they are tall, and a proportional font much more so)
        // and the block is a rectangle nothing else reads, so over-reserving costs nothing while
        // under-reserving would cut the text off. Written at the object's own first frame, since
        // the string it measures does not change over the object's life.
        private static void ApplyTextSize(RectObject target, InteropReport report, string path)
        {
            if (target is not TextObject text) return;

            report.Approximated("text_bounds_estimated",
                "Afterbeat text has no bounds; imported text was given a block one character wide per character and one line tall per line, which fits any typeface rather than matching the source exactly.",
                path);

            if (text.Sizes.Count > 0) text.Sizes.Clear();

            var (columns, lines) = MeasureText((text.Text as StringValue)?.Value);
            text.Sizes.Add(new ScaKey(
                new Vector2Value(columns * TextColumnWidth, lines * TextLineHeight),
                FrameRules.MinFrame, EaseType.Linear));
        }

        /// <summary> World units one character of an imported text is given. </summary>
        public const float TextColumnWidth = 1f;

        /// <summary> World units one line of an imported text is given. </summary>
        public const float TextLineHeight = 1f;

        // Inline formatting tags are not text and must not be measured - a line carrying a colour
        // tag is not sixty characters wide because the tag spelled it that way. Everything else is
        // counted as written, including whitespace.
        private static (int Columns, int Lines) MeasureText(string value)
        {
            if (string.IsNullOrEmpty(value)) return (1, 1);

            var lines = 1;
            var longest = 0;
            var current = 0;
            var inTag = false;

            foreach (var character in value)
            {
                switch (character)
                {
                    case '<':
                        inTag = true;
                        continue;
                    case '>' when inTag:
                        inTag = false;
                        continue;
                    case '\n':
                        if (current > longest) longest = current;
                        current = 0;
                        lines++;
                        continue;
                }

                if (inTag) continue;
                if (character != '\r') current++;
            }

            if (current > longest) longest = current;
            return (Math.Max(1, longest), Math.Max(1, lines));
        }

        private static RectObject CreateText(VgdObject source, ABImportContext context, string path)
        {
            if (!string.IsNullOrEmpty(source.Text) && source.Text.IndexOf('<') >= 0)
                context.Report.Approximated("text_inline_tags",
                    "Afterbeat text carries inline formatting tags this format does not interpret; they were kept as literal text.",
                    path);

            return new TextObject
            {
                Text = new StringValue(source.Text ?? string.Empty),
            };
        }

        #endregion

        #region Layer and pivot

        // Draw order itself is ABLayerMap's; only the conversion into what this format stores
        // happens here, and it is a subtraction. Layer is parent-relative here and the resolved
        // layer is absolute, so a child stores the difference.
        //
        // The parent's effective layer comes from the table the first pass filled, never from
        // whatever this pass happens to have seen: an object list is in no particular order, so
        // computing it as it goes gives every child written before its parent a parent layer of
        // zero and draws that whole branch at the wrong depth.
        private static void ApplyLayer(VgdObject source, RectObject target,
            ABImportContext context, int effectiveLayer)
        {
            var parentEffective = context.GetParentEffectiveLayer(source.ParentId);
            var relative = effectiveLayer - parentEffective;

            target.Layer = Math.Clamp(relative, ValueRules.MinLayer, ValueRules.MaxLayer);
        }

        // Afterbeat's origin is an OFFSET of the reference point from the object's centre; this
        // format's pivot is a normalized point inside the object's own box, with 0.5,0.5 at the
        // centre. Moving the reference point one way moves the pivot the other, hence the
        // subtraction. An origin of zero is the ordinary case and converts exactly, which is why it
        // is not reported.
        //
        // TEXT IS THE OPPOSITE SIGN, and it is not a special case invented here - it is a different
        // mechanism over there. A shape's origin moves the MESH by +origin under an unmoved
        // transform, so the transform sits at 0.5 - origin of the shape. A text's origin instead
        // picks one of TextMeshPro's three alignments, and an origin of +0.5 selects Right - the
        // glyph run pushed against the right edge of the block, extending LEFT. Subtracting there
        // anchors the text at the left edge and runs it the other way, i.e. the reading direction
        // of every off-centre text in a converted level was mirrored.
        private static void ApplyPivot(VgdObject source, RectObject target,
            InteropReport report, string path)
        {
            var originX = source.Origin?.X ?? 0f;
            var originY = source.Origin?.Y ?? 0f;
            var authored = originX != 0f || originY != 0f;

            if (target is TextObject)
            {
                if (!authored) return;

                report.Approximated("origin_text_alignment",
                    "Afterbeat lays text out by one of three alignments rather than by a pivot; an origin between them is centred there and is placed proportionally here.",
                    path);

                target.Pivots.Add(new AlignmentKey(
                    new Vector2Value(DefaultPivot + originX, DefaultPivot + originY),
                    FrameRules.MinFrame));
                return;
            }

            // A shape whose geometry is offset inside its own box carries that offset as a pivot
            // too, and the two ADD: an author can move the reference point of a Triangle Bottom
            // exactly as they can any other shape's, and reading either alone puts it in the wrong
            // place. It is NOT reported, unlike an authored origin - nothing was approximated,
            // this is simply where that shape sits.
            originY += ABShapeMap.GetPivotOffsetY(source.Shape, source.ShapeOption);

            if (originX == 0f && originY == 0f) return;

            if (authored)
                report.Approximated("origin_pivot",
                    "Afterbeat's object origin was converted into this format's pivot; the two measure the same idea from opposite sides, so check objects whose origin was not centred.",
                    path);

            target.Pivots.Add(new AlignmentKey(
                new Vector2Value(DefaultPivot - originX, DefaultPivot - originY), FrameRules.MinFrame));
        }

        /// <summary> Centre of an object's own box, in this format's normalized pivot space. </summary>
        public const float DefaultPivot = 0.5f;

        // A PARENT'S ORIGIN MUST NOT REACH ITS CHILDREN, and here it does unless this runs. Over
        // there the origin lives on a leaf mesh transform hanging below the visual object, so the
        // parent chain never sees it; here it is the parent's Pivot, and RectTransform2D.Apply
        // computes a child's frame from a centre point that the pivot moves - so every child of an
        // off-centre parent is displaced by it, by more the larger the parent is.
        //
        // The cancellation is structural rather than baked, which is what makes it survive an
        // animated parent with no keyframes added: Apply's anchor term is
        // Rot(r_p) * ((lerp(anchorMin, anchorMax, pivot) - 0.5) * parentFullSize), and with the two
        // anchors EQUAL the pivot drops out of the lerp entirely, leaving a term that cancels the
        // centre-point shift for any parent size and any parent rotation. Equal anchors also leave
        // the `size += parent.size * (max - min)` term at zero, so nothing else moves.
        private static void ApplyParentOrigin(VgdObject source, RectObject target,
            IReadOnlyDictionary<string, VgdObject> byId)
        {
            if (string.IsNullOrEmpty(source.ParentId)) return;
            if (!byId.TryGetValue(source.ParentId, out var parent) || parent == null) return;

            // Whatever moved the PARENT's pivot has to be cancelled, whether the author moved it or
            // the parent's own shape sits off-centre in its box - both end up on the same field, so
            // both leak the same way. Text is excluded because its pivot went the other way.
            var originX = parent.Origin?.X ?? 0f;
            var originY = parent.Origin?.Y ?? 0f;

            if (!ABShapeMap.IsText(parent.Shape))
                originY += ABShapeMap.GetPivotOffsetY(parent.Shape, parent.ShapeOption);
            else
                (originX, originY) = (-originX, -originY);

            if (originX == 0f && originY == 0f) return;

            var anchor = new Vector2Value(DefaultPivot - originX, DefaultPivot - originY);
            target.AnchorsMin.Add(new AlignmentKey(anchor, FrameRules.MinFrame));
            target.AnchorsMax.Add(new AlignmentKey(anchor, FrameRules.MinFrame));
        }

        // What is reported here is what is LOST, and the condition used to be the exact inverse of
        // that. It fired on a mask that was not the format's own default - i.e. on "111", the one
        // mask this format expresses perfectly - and stayed silent on the default "101", which is
        // both the most common mask in real levels and a genuine loss. So every level came back
        // with a finding on the objects that were fine and nothing on the objects that were not.
        //
        // Two of the three bits are answered elsewhere rather than reported: the SCALE bit crosses
        // exactly through the choice of Size vs Scale (ResolveScaleTargets), and a mask of "000"
        // crosses exactly by importing the object as a root. What is left is a mask that drops the
        // parent's position or rotation while keeping something else - which needs the parent's
        // value at each sample time to cancel, and is therefore not something this format can hold.
        private static void ReportParenting(VgdObject source, InteropReport report, string path)
        {
            if (string.IsNullOrEmpty(source.ParentId)) return;
            if (InheritsNothing(source)) return;

            if (!HasParentBit(source, ParentTypeIndex.Position))
                report.Dropped("parent_position_not_inherited",
                    "Afterbeat can stop a child following its parent's position while it still follows the rest; this format inherits the whole transform, so those objects move with their parent here.",
                    path);

            if (!HasParentBit(source, ParentTypeIndex.Rotation))
                report.Dropped("parent_rotation_not_inherited",
                    "Afterbeat can stop a child following its parent's rotation while it still follows the rest; this format inherits the whole transform, so those objects turn with their parent here.",
                    path);

            if (source.ParentOffsets == null) return;
            foreach (var offset in source.ParentOffsets)
            {
                if (offset == 0f) continue;
                report.Dropped("parent_time_offset",
                    "Afterbeat can delay a child's inheritance from its parent in time; this format has no equivalent, so those delays are not imported.",
                    path);
                return;
            }
        }

        #endregion

        #region Tracks

        private static void ImportPositions(VgdObject source, RectObject target, int framerate,
            ABImportContext context, string path)
        {
            var track = source.Move;
            if (track?.Keyframes == null) return;

            var report = context.Report;
            var compensation = GetCompensation(source, context);

            foreach (var key in Take(track.Keyframes, LevelRules.MaxObjectKeys, report, path))
            {
                var value = ABValueMap.ImportVector(
                    key.GetValue(0) * compensation.X, key.GetValue(1) * compensation.Y,
                    key, report, path);
                target.Positions.Add(new PosKey(value, LocalFrame(key, framerate),
                    ABEaseMap.Import(key.Ease, report, path)));
            }
            Deduplicate(target.Positions, k => k.Frame, report, path);
        }

        // WHICH FIELD the source scale lands in decides whether this object's children inherit it,
        // and getting that wrong is what pulls a whole parented hierarchy apart - see
        // ABImportContext.ScaleTarget. Everything below the text exception follows from the table
        // ResolveScaleTargets built.
        //
        // Text is the one object whose field is fixed regardless, and the reason is on the other
        // side: an Afterbeat text object has a scale and NO font size, so its scale is the only
        // thing sizing the glyphs, while here Size is the block the glyphs are laid out in and
        // Scale is the multiplier on top of it. Writing the source scale into Size gave every
        // imported text a one-by-one block - a whole line of text inside a single cell - which is
        // what ApplyTextSize fixes. It also happens to be the propagating field, so a text object
        // parenting something behaves as p_t[1] == '1' whatever its children asked for; a text
        // object with children is rare enough to leave at that.
        private static void ImportScales(VgdObject source, RectObject target, int framerate,
            ABImportContext context, string path)
        {
            var track = source.Scale;
            if (track?.Keyframes == null) return;

            var report = context.Report;
            var toScale = target is TextObject
                          || context.GetScaleTarget(source.Id) == ABImportContext.ScaleTarget.Scale;
            var into = toScale ? target.Scales : target.Sizes;
            var compensation = GetCompensation(source, context);

            // A custom polygon whose geometry had to be shrunk to fit this format's own box is drawn
            // back up here - see ABShapeMap.GetCustomSizeCompensation. It multiplies into the SIZE
            // and never into the scale, since the object grew and its children did not.
            var shapeFit = ABShapeMap.GetCustomSizeCompensation(source);
            var sizeFactor = toScale ? 1f : shapeFit;

            // The axis trade is a SCALE correction and belongs nowhere near the position - see
            // ResolveAxisSwaps.
            var swap = GetAxisSwap(source, context);

            foreach (var key in Take(track.Keyframes, LevelRules.MaxObjectKeys, report, path))
            {
                var value = ABValueMap.ImportVector(
                    key.GetValue(0) * compensation.X * sizeFactor * swap.X,
                    key.GetValue(1) * compensation.Y * sizeFactor * swap.Y,
                    key, report, path);
                into.Add(new ScaKey(value, LocalFrame(key, framerate),
                    ABEaseMap.Import(key.Ease, report, path)));
            }
            Deduplicate(into, k => k.Frame, report, path);

            // The object's own scale went to Scales, so the shape's shrink has nowhere to be undone
            // except a Size of its own - which is empty here and would otherwise fall back to one.
            if (toScale && target is not TextObject && Math.Abs(shapeFit - 1f) > float.Epsilon)
                target.Sizes.Add(new ScaKey(new Vector2Value(shapeFit, shapeFit), FrameRules.MinFrame));
        }

        // A child whose own mask disagreed with the field its parent's scale had to go into. Both
        // of its own tracks are scaled by the same factor, since the parent's scale reaches the
        // child's OFFSET as well as its extent.
        private static (float X, float Y) GetCompensation(VgdObject source, ABImportContext context)
            => source?.Id != null
               && context.ScaleCompensations.TryGetValue(source.Id, out var factor)
                ? factor
                : (1f, 1f);

        private static (float X, float Y) GetAxisSwap(VgdObject source, ABImportContext context)
            => source?.Id != null
               && context.AxisSwaps.TryGetValue(source.Id, out var factor)
                ? factor
                : (1f, 1f);

        // The one track that cannot be converted keyframe by keyframe: each source value is a delta
        // from the one before it, so the whole track has to be walked in order while a running total
        // is kept.
        private static void ImportRotations(VgdObject source, RectObject target, int framerate,
            InteropReport report, string path)
        {
            var track = source.Rotate;
            if (track?.Keyframes == null) return;

            var accumulated = 0f;
            foreach (var key in Take(track.Keyframes, LevelRules.MaxObjectKeys, report, path))
            {
                var radians = ABValueMap.AccumulateRotation(key.GetValue(0), ref accumulated);
                target.Rotations.Add(new AngleKey(new FloatValue(radians), LocalFrame(key, framerate),
                    ABEaseMap.Import(key.Ease, report, path)));
            }
            Deduplicate(target.Rotations, k => k.Frame, report, path);
        }

        private static void ImportColors(VgdObject source, RectObject target,
            ABImportContext context, string path)
        {
            var track = source.Color;
            if (track?.Keyframes == null) return;

            var report = context.Report;
            var framerate = context.Options.Framerate;
            var gradient = (ABGradientType)source.GradientType;

            switch (target)
            {
                case ShapeObject shape:
                {
                    foreach (var key in Take(track.Keyframes, LevelRules.MaxObjectKeys, report, path))
                        shape.Colors.Add(BuildShapeColor(key, gradient, context, path));
                    Deduplicate(shape.Colors, k => k.Frame, report, path);
                    break;
                }
                case TextObject text:
                {
                    foreach (var key in Take(track.Keyframes, LevelRules.MaxObjectKeys, report, path))
                        text.Colors.Add(new Color4Key(ReadStartColor(key, context, path),
                            LocalFrame(key, framerate), ABEaseMap.Import(key.Ease, report, path)));
                    Deduplicate(text.Colors, k => k.Frame, report, path);
                    break;
                }
                default:
                    // An Empty object draws nothing, so its colour track describes nothing. Not a
                    // loss worth reporting - the source level did not draw it either.
                    break;
            }
        }

        // A linear gradient becomes a two-corner colour, which is the closest this format has; its
        // ROTATION and SCALE have no equivalent and are reported once. A radial gradient has no
        // shape here at all and falls back to its start colour.
        private static IColor4X4Key BuildShapeColor(VgdKeyframe key, ABGradientType gradient,
            ABImportContext context, string path)
        {
            var report = context.Report;
            var frame = LocalFrame(key, context.Options.Framerate);
            var ease = ABEaseMap.Import(key.Ease, report, path);
            var start = ReadStartColor(key, context, path);

            switch (gradient)
            {
                case ABGradientType.Linear:
                case ABGradientType.InvertedLinear:
                {
                    report.Approximated("gradient_linear",
                        "Afterbeat's linear object gradient became a two-corner colour; its rotation and scale have no equivalent here.",
                        path);
                    var end = ReadEndColor(key, context, path);
                    return gradient == ABGradientType.Linear
                        ? new ColorHorizontalKey(start, end, frame, ease)
                        : new ColorHorizontalKey(end, start, frame, ease);
                }

                case ABGradientType.Radial:
                case ABGradientType.InvertedRadial:
                    report.Dropped("gradient_radial",
                        "Afterbeat's radial object gradient has no equivalent here; those objects use their start colour flat.",
                        path);
                    return new Color4Key(start, frame, ease);

                default:
                    return new Color4Key(start, frame, ease);
            }
        }

        private static IColor4 ReadStartColor(VgdKeyframe key, ABImportContext context, string path)
            => ABColorMap.Import((int)key.GetValue(0), OpacityOf(key), ABPalette.Objects,
                context.ReferenceTheme, context.Report, path);

        private static IColor4 ReadEndColor(VgdKeyframe key, ABImportContext context, string path)
            => ABColorMap.Import((int)key.GetValue(2), OpacityOf(key), ABPalette.Objects,
                context.ReferenceTheme, context.Report, path);

        // Afterbeat writes opacity as a PERCENTAGE, 0 to 100, and this format stores alpha as 0 to
        // 1. Read straight across, every fade an author wrote clamped to fully opaque, so a level
        // arrived with every one of its fades missing - the single most visible colour bug in the
        // converter, and invisible in a round trip because both directions had it.
        //
        // A keyframe that carries only its index is fully opaque: the format's own default for a
        // missing component is 0, which here would mean invisible, and the source game's own reader
        // fills a missing opacity with 100 rather than with 0.
        public const float OpacityScale = 100f;

        private static float OpacityOf(VgdKeyframe key)
            => key.Values != null && key.Values.Count > 1 ? key.GetValue(1) / OpacityScale : 1f;

        #endregion

        #region Shared

        /// <summary> A keyframe's frame, local to its object exactly as it was local to its object
        /// in the source. </summary>
        private static int LocalFrame(VgdKeyframe key, int framerate)
            => ABTimeMap.ToFrame(key.Time, framerate);

        // This format caps a track at LevelRules.MaxObjectKeys and Afterbeat does not, so a long
        // track is truncated rather than thinned: dropping every other key changes the motion
        // everywhere, while cutting the tail leaves everything before it exactly as authored.
        //
        // Ordering is this method's job because two readers below depend on it and neither can
        // check: rotation accumulates each delta onto the one before it, so an out-of-order track
        // integrates to a different animation, and "the tail" is only the tail if the keys are in
        // time order. The format guarantees keyframe times are unique, never that they are sorted.
        private static IEnumerable<VgdKeyframe> Take(List<VgdKeyframe> keyframes, int max,
            InteropReport report, string path)
        {
            var sorted = new List<VgdKeyframe>(keyframes);
            sorted.Sort(CompareByTime);

            var taken = 0;
            foreach (var key in sorted)
            {
                if (key == null) continue;
                if (taken >= max)
                {
                    report.Dropped("keys_over_cap",
                        $"Some tracks carry more than {max} keyframes, which is this format's limit; the extra ones were dropped.",
                        path);
                    yield break;
                }
                taken++;
                yield return key;
            }
        }

        private static int CompareByTime(VgdKeyframe left, VgdKeyframe right)
        {
            if (left == null) return right == null ? 0 : 1;
            if (right == null) return -1;
            return left.Time.CompareTo(right.Time);
        }

        // Two source keyframes can round onto one frame, and this format forbids that outright
        // (RuleCollectionUnique). The LATER one wins, matching how a timeline behaves when a key is
        // dragged onto another.
        private static void Deduplicate<T>(List<T> keyframes, Func<T, int> frameOf,
            InteropReport report, string path)
        {
            if (keyframes == null || keyframes.Count < 2) return;

            var latest = new Dictionary<int, T>(keyframes.Count);
            var order = new List<int>(keyframes.Count);
            var dropped = false;

            foreach (var keyframe in keyframes)
            {
                var frame = frameOf(keyframe);
                if (latest.ContainsKey(frame)) dropped = true;
                else order.Add(frame);
                latest[frame] = keyframe;
            }

            if (dropped)
            {
                keyframes.Clear();
                foreach (var frame in order) keyframes.Add(latest[frame]);


                report.Approximated("keys_collided",
                    "Some keyframes landed on the same frame once converted from seconds; the later one was kept. A higher framerate keeps them apart.",
                    path);
            }
        }

        #endregion
    }
}
