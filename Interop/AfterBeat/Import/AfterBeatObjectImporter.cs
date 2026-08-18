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
    //    halves are done by AfterBeatValueMap; either one missed produces a level that spins.
    // 3. Depth is absolute there and Layer is parent-relative here, so a child's layer is its own
    //    effective layer minus its parent's. Reading obj.d straight into Layer draws a whole
    //    hierarchy at the wrong depth as soon as anything is parented.
    // 4. Afterbeat's "scale" is a width/height in world units, which is this format's SIZE, not its
    //    Scale. Scale is left empty, i.e. at the engine's own 1x1 fallback.
    //
    // Import is TWO passes over the object list on purpose - see AfterBeatImportContext.

    /// <summary> One Afterbeat object into one <see cref="RectObject"/>. </summary>
    public static class AfterBeatObjectImporter
    {
        /// <summary> Mints ids for every source object, then fills them in. Ids first, because a
        /// child may be written before its parent. </summary>
        public static void ImportAll(IReadOnlyList<VgdObject> sources, AfterBeatImportContext context,
            string pathPrefix)
        {
            if (sources == null || context?.Scope?.Objects == null) return;

            foreach (var source in sources)
            {
                if (source == null) continue;
                context.Mint(source.Id);
            }

            for (var i = 0; i < sources.Count; i++)
            {
                var source = sources[i];
                if (source == null) continue;

                var path = $"{pathPrefix}[{i}]";
                var imported = Import(source, context, path);
                if (imported == null) continue;

                context.Scope.Objects[imported.ObjectId] = imported;
            }
        }

        /// <summary> One object. Returns null only when the source is null. </summary>
        public static RectObject Import(VgdObject source, AfterBeatImportContext context, string path)
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
            target.Span = AfterBeatTimeMap.ResolveSpan(source, framerate, report, path);
            target.ParentObjectId = context.ResolveParent(source.ParentId, path);

            ApplyLayer(source, target, context);
            ApplyPivot(source, target, report, path);
            ReportParenting(source, report, path);

            ImportPositions(source, target, framerate, report, path);
            ImportScales(source, target, framerate, report, path);
            ImportRotations(source, target, framerate, report, path);
            ImportColors(source, target, context, path);

            return target;
        }

        #region Type

        // Hit and No Hit are the same object with and without a collider, which is exactly how this
        // format expresses the distinction. Empty carries no geometry at all and becomes the base
        // type - it is a transform other objects hang off, and giving it a shape would draw
        // something the source level never drew.
        private static RectObject CreateTarget(VgdObject source, AfterBeatImportContext context, string path)
        {
            if (AfterBeatShapeMap.IsText(source.Shape))
                return CreateText(source, context, path);

            if ((AfterBeatObjectType)source.ObjectType == AfterBeatObjectType.Empty)
                return new RectObject();

            var shapeId = AfterBeatShapeMap.Import(source.Shape, source.ShapeOption,
                context.Shapes, context.Report, path);

            return new ShapeObject
            {
                ShapeId = shapeId,
                ColliderId = (AfterBeatObjectType)source.ObjectType == AfterBeatObjectType.Hit
                    ? shapeId
                    : ShapeId.Null,
                ShaderType = ShaderType.Auto,
            };
        }

        private static RectObject CreateText(VgdObject source, AfterBeatImportContext context, string path)
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

        // Afterbeat draws SMALLER depth in front; this format draws HIGHER layer in front. Their
        // default depth (20) is mapped onto this format's default layer (0), so an ordinary object
        // imports at an ordinary layer instead of at -20.
        private static void ApplyLayer(VgdObject source, RectObject target, AfterBeatImportContext context)
        {
            var effective = VgdObject.DefaultDepth - source.Depth;
            var parentEffective = context.GetParentEffectiveLayer(source.ParentId);

            target.Layer = Math.Clamp(effective - parentEffective, ValueRules.MinLayer, ValueRules.MaxLayer);
            if (!string.IsNullOrEmpty(source.Id)) context.EffectiveLayers[source.Id] = effective;
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
                var value = AfterBeatValueMap.ImportVector(key.GetValue(0), key.GetValue(1), key, report, path);
                target.Positions.Add(new PosKey(value, LocalFrame(key, framerate),
                    AfterBeatEaseMap.Import(key.Ease, report, path)));
            }
            Deduplicate(target.Positions, k => k.Frame, report, path);
        }

        private static void ImportScales(VgdObject source, RectObject target, int framerate,
            InteropReport report, string path)
        {
            var track = source.Scale;
            if (track?.Keyframes == null) return;

            foreach (var key in Take(track.Keyframes, LevelRules.MaxObjectKeys, report, path))
            {
                var value = AfterBeatValueMap.ImportVector(key.GetValue(0), key.GetValue(1), key, report, path);
                target.Sizes.Add(new ScaKey(value, LocalFrame(key, framerate),
                    AfterBeatEaseMap.Import(key.Ease, report, path)));
            }
            Deduplicate(target.Sizes, k => k.Frame, report, path);
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
                var radians = AfterBeatValueMap.AccumulateRotation(key.GetValue(0), ref accumulated);
                target.Rotations.Add(new AngleKey(new FloatValue(radians), LocalFrame(key, framerate),
                    AfterBeatEaseMap.Import(key.Ease, report, path)));
            }
            Deduplicate(target.Rotations, k => k.Frame, report, path);
        }

        private static void ImportColors(VgdObject source, RectObject target,
            AfterBeatImportContext context, string path)
        {
            var track = source.Color;
            if (track?.Keyframes == null) return;

            var report = context.Report;
            var framerate = context.Options.Framerate;
            var gradient = (AfterBeatGradientType)source.GradientType;

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
                            LocalFrame(key, framerate), AfterBeatEaseMap.Import(key.Ease, report, path)));
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
        private static IColor4X4Key BuildShapeColor(VgdKeyframe key, AfterBeatGradientType gradient,
            AfterBeatImportContext context, string path)
        {
            var report = context.Report;
            var frame = LocalFrame(key, context.Options.Framerate);
            var ease = AfterBeatEaseMap.Import(key.Ease, report, path);
            var start = ReadStartColor(key, context, path);

            switch (gradient)
            {
                case AfterBeatGradientType.Linear:
                case AfterBeatGradientType.InvertedLinear:
                {
                    report.Approximated("gradient_linear",
                        "Afterbeat's linear object gradient became a two-corner colour; its rotation and scale have no equivalent here.",
                        path);
                    var end = ReadEndColor(key, context, path);
                    return gradient == AfterBeatGradientType.Linear
                        ? new ColorHorizontalKey(start, end, frame, ease)
                        : new ColorHorizontalKey(end, start, frame, ease);
                }

                case AfterBeatGradientType.Radial:
                case AfterBeatGradientType.InvertedRadial:
                    report.Dropped("gradient_radial",
                        "Afterbeat's radial object gradient has no equivalent here; those objects use their start colour flat.",
                        path);
                    return new Color4Key(start, frame, ease);

                default:
                    return new Color4Key(start, frame, ease);
            }
        }

        private static IColor4 ReadStartColor(VgdKeyframe key, AfterBeatImportContext context, string path)
            => AfterBeatColorMap.Import((int)key.GetValue(0), OpacityOf(key), AfterBeatPalette.Objects,
                context.ReferenceTheme, context.Report, path);

        private static IColor4 ReadEndColor(VgdKeyframe key, AfterBeatImportContext context, string path)
            => AfterBeatColorMap.Import((int)key.GetValue(2), OpacityOf(key), AfterBeatPalette.Objects,
                context.ReferenceTheme, context.Report, path);

        // A colour keyframe that carries only its index is fully opaque; the format's own default
        // for a missing component is 0, which here would mean invisible.
        private static float OpacityOf(VgdKeyframe key)
            => key.Values != null && key.Values.Count > 1 ? key.GetValue(1) : 1f;

        #endregion

        #region Shared

        /// <summary> A keyframe's frame, local to its object exactly as it was local to its object
        /// in the source. </summary>
        private static int LocalFrame(VgdKeyframe key, int framerate)
            => AfterBeatTimeMap.ToFrame(key.Time, framerate);

        // This format caps a track at LevelRules.MaxObjectKeys and Afterbeat does not, so a long
        // track is truncated rather than thinned: dropping every other key changes the motion
        // everywhere, while cutting the tail leaves everything before it exactly as authored.
        private static IEnumerable<VgdKeyframe> Take(List<VgdKeyframe> keyframes, int max,
            InteropReport report, string path)
        {
            var taken = 0;
            foreach (var key in keyframes)
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
