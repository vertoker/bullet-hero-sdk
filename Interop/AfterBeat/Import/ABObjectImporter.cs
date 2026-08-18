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

            for (var i = 0; i < sources.Count; i++)
            {
                var source = sources[i];
                if (source == null) continue;

                var path = $"{pathPrefix}[{i}]";
                var imported = Import(source, context, layers.Layers[i], path);
                if (imported == null) continue;

                context.Scope.Objects[imported.ObjectId] = imported;
            }
        }

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
            target.ParentObjectId = context.ResolveParent(source.ParentId, path);

            ApplyLayer(source, target, context, effectiveLayer);
            ApplyPivot(source, target, report, path);
            ReportParenting(source, report, path);

            ImportPositions(source, target, framerate, report, path);
            ImportScales(source, target, framerate, report, path);
            ApplyTextSize(target, report, path);
            ImportRotations(source, target, framerate, report, path);
            ImportColors(source, target, context, path);

            return target;
        }

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
        private static void ApplyPivot(VgdObject source, RectObject target,
            InteropReport report, string path)
        {
            var originX = source.Origin?.X ?? 0f;
            var originY = source.Origin?.Y ?? 0f;
            if (originX == 0f && originY == 0f) return;

            report.Approximated("origin_pivot",
                "Afterbeat's object origin was converted into this format's pivot; the two measure the same idea from opposite sides, so check objects whose origin was not centred.",
                path);

            target.Pivots.Add(new AlignmentKey(
                new Vector2Value(DefaultPivot - originX, DefaultPivot - originY), FrameRules.MinFrame));
        }

        /// <summary> Centre of an object's own box, in this format's normalized pivot space. </summary>
        public const float DefaultPivot = 0.5f;

        private static void ReportParenting(VgdObject source, InteropReport report, string path)
        {
            if (string.IsNullOrEmpty(source.ParentId)) return;

            if (!string.IsNullOrEmpty(source.ParentType) && source.ParentType != VgdObject.DefaultParentType)
                report.Dropped("parent_channel_mask",
                    "Afterbeat can inherit position, scale and rotation from a parent independently; this format inherits all three together, so those switches are not imported.",
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
            InteropReport report, string path)
        {
            var track = source.Move;
            if (track?.Keyframes == null) return;

            foreach (var key in Take(track.Keyframes, LevelRules.MaxObjectKeys, report, path))
            {
                var value = ABValueMap.ImportVector(key.GetValue(0), key.GetValue(1), key, report, path);
                target.Positions.Add(new PosKey(value, LocalFrame(key, framerate),
                    ABEaseMap.Import(key.Ease, report, path)));
            }
            Deduplicate(target.Positions, k => k.Frame, report, path);
        }

        private static void ImportScales(VgdObject source, RectObject target, int framerate,
            InteropReport report, string path)
        {
            var track = source.Scale;
            if (track?.Keyframes == null) return;

            // For everything but text the source's scale IS this format's size, in world units.
            // Text is the exception and the reason is on the other side: an Afterbeat text object
            // has a scale and NO font size, so its scale is the only thing sizing the glyphs, while
            // here Size is the block the glyphs are laid out in and Scale is the multiplier on top
            // of it. Writing the source scale into Size gave every imported text a one-by-one block
            // - a whole line of text inside a single cell - which is what ApplyTextSize fixes.
            var into = target is TextObject ? target.Scales : target.Sizes;

            foreach (var key in Take(track.Keyframes, LevelRules.MaxObjectKeys, report, path))
            {
                var value = ABValueMap.ImportVector(key.GetValue(0), key.GetValue(1), key, report, path);
                into.Add(new ScaKey(value, LocalFrame(key, framerate),
                    ABEaseMap.Import(key.Ease, report, path)));
            }
            Deduplicate(into, k => k.Frame, report, path);
        }

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
