using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat.Import
{
    /// <summary>
    /// Afterbeat refuses damage from any object whose rendered alpha is below 1, and this pass is
    /// what carries that rule across. It runs after an object is imported and rewrites WHEN its
    /// collider exists, never what it draws.
    ///
    /// THE RULE, from the source game rather than from the wiki (VGPlayer.CheckForObjectCollision):
    /// the collider is never disabled over there - the damage check itself reads the material and
    /// returns false when `_BaseColor.a &lt; 1` (or `_Alpha &lt; 1` on the gradient shader). Opacity
    /// reaches those two properties and nothing else. So the threshold is NOT zero: an object at 99%
    /// is already harmless, and an object authored at a constant 35% is decoration for its whole
    /// life. Authors lean on this constantly without ever saying so - a splash that fades out is
    /// intangible for the whole fade, which is why fading is how splashes are ended over there.
    ///
    /// WHY IT NEEDS EXTRA OBJECTS: this format's ColliderId is a per-object constant with no
    /// keyframe track behind it, so "hits between these frames and not those" is not expressible on
    /// one object. An object whose opacity crosses the boundary therefore gives up its own collider
    /// and gains one invisible child per opaque stretch (ShapeId Null + a real ColliderId, which is
    /// ordinary authored content here). The child is anchor-stretched over its parent's whole rect,
    /// so it inherits the parent's motion, size, rotation and Active flag for free, and its hitbox is
    /// exactly the parent's - collision and rendering share one mapping, so nothing has to be
    /// recomputed to keep the two aligned.
    ///
    /// WHAT IT DELIBERATELY DOES NOT DO: an object opaque for its whole life is left untouched, which
    /// is the overwhelming majority of every level, so nothing pays for this but the levels that use
    /// it. And the boundary is read from the SOURCE's own opacity percentages rather than from the
    /// imported alpha: the rule is defined over there in percent against 100, and re-deriving it from
    /// a colour that may have become a theme reference would be answering a different question.
    /// </summary>
    public static class ABOpacityHitGate
    {
        /// <summary> One source opacity keyframe, reduced to the two things this rule reads. </summary>
        public readonly struct OpacitySample
        {
            public readonly int Frame;
            public readonly float Opacity;

            public OpacitySample(int frame, float opacity)
            {
                Frame = frame;
                Opacity = opacity;
            }
        }

        /// <summary> A stretch of an object's own life, local to its span. </summary>
        public readonly struct FrameRange
        {
            public readonly int Start;
            public readonly int Duration;

            public FrameRange(int start, int duration)
            {
                Start = start;
                Duration = duration;
            }
        }

        // Alpha is compared against 1 with no epsilon, exactly as the source game compares it: the
        // value there is opacity/100 of an integer percentage, so every legal value is either 1
        // exactly or visibly below it, and a tolerance would only invent a band where the two
        // implementations disagree.
        private const float OpaqueOpacity = 1f;

        /// <summary> Rewrites one imported object's collider to exist only while the source was fully
        /// opaque. Adds the extra collider children to the scope; a no-op for anything that carries
        /// no collider or never fades. </summary>
        public static void Apply(VgdObject source, RectObject target, ABImportContext context, string path)
        {
            if (source == null || context?.Scope?.Objects == null) return;
            if (target is not ShapeObject shape) return;
            if (!shape.ColliderId.IsEnabled()) return;

            // No colour track at all is not "invisible": the source game's own reader fills a
            // missing opacity with 100, the same call ABObjectImporter.OpacityOf makes.
            var samples = CollectSamples(source, context.Options.Framerate);
            if (samples.Count == 0) return;

            var duration = target.Span.FrameDuration;
            var ranges = ResolveOpaqueRanges(samples, duration);

            if (IsWholeSpan(ranges, duration)) return;

            var colliderId = shape.ColliderId;
            shape.ColliderId = ShapeId.Null;

            if (ranges.Count == 0)
            {
                context.Report.Approximated("collider_opacity_never_opaque",
                    "Objects that never reach full opacity cannot hurt the player in the source game, so they were imported without a collider.",
                    path);
                return;
            }

            foreach (var range in ranges)
            {
                var child = BuildColliderChild(shape, colliderId, range, context);
                context.Scope.Objects[child.ObjectId] = child;
            }

            context.Report.Approximated("collider_opacity_gate",
                "Objects only hurt the player at full opacity in the source game; a faded object was imported as a collider-less object plus one invisible collider per fully opaque stretch.",
                path);
        }

        // Held before the first key and after the last one, exactly as the source game holds a
        // sequence's ends, and interpolated in between - which is why a segment counts as opaque
        // only when BOTH of its ends are. A fade therefore stops hitting on the keyframe it starts
        // from rather than one frame later: half a frame earlier than the source game, and in the
        // direction that cannot kill a player who saw the object go transparent.

        /// <summary> The stretches of [0, duration) over which the source was fully opaque. </summary>
        public static List<FrameRange> ResolveOpaqueRanges(IReadOnlyList<OpacitySample> samples, int duration)
        {
            var ranges = new List<FrameRange>();
            if (samples == null || samples.Count == 0 || duration <= 0) return ranges;

            var start = -1;
            var end = -1;

            // The first key's value is held backwards to the object's own start, so a track whose
            // first key sits late has one more stretch than it has keys.
            if (samples[0].Frame > 0)
                Append(ranges, ref start, ref end, 0, Clamp(samples[0].Frame, duration),
                    samples[0].Opacity >= OpaqueOpacity);

            for (var i = 0; i < samples.Count; i++)
            {
                var from = Clamp(samples[i].Frame, duration);
                var to = i + 1 < samples.Count ? Clamp(samples[i + 1].Frame, duration) : duration;
                var opaque = samples[i].Opacity >= OpaqueOpacity
                             && (i + 1 >= samples.Count || samples[i + 1].Opacity >= OpaqueOpacity);

                Append(ranges, ref start, ref end, from, to, opaque);
            }

            if (start >= 0) ranges.Add(new FrameRange(start, end - start));
            return ranges;
        }

        // One stretch at a time, merged into the one before it when both are opaque and they touch -
        // a track of ten opaque keys is one range, not ten adjacent objects.
        private static void Append(ICollection<FrameRange> ranges, ref int start, ref int end,
            int from, int to, bool opaque)
        {
            if (to <= from) return;

            if (opaque)
            {
                if (start < 0) { start = from; end = to; return; }
                if (from <= end) { end = to; return; }

                ranges.Add(new FrameRange(start, end - start));
                start = from;
                end = to;
                return;
            }

            if (start < 0) return;
            ranges.Add(new FrameRange(start, end - start));
            start = -1;
            end = -1;
        }

        private static int Clamp(int frame, int duration)
            => frame < 0 ? 0 : frame > duration ? duration : frame;

        private static bool IsWholeSpan(IReadOnlyList<FrameRange> ranges, int duration)
            => ranges.Count == 1 && ranges[0].Start == 0 && ranges[0].Duration >= duration;

        private static List<OpacitySample> CollectSamples(VgdObject source, int framerate)
        {
            var samples = new List<OpacitySample>();
            var keyframes = source.Color?.Keyframes;
            if (keyframes == null) return samples;

            foreach (var key in keyframes)
            {
                if (key == null) continue;
                var opacity = key.Values != null && key.Values.Count > 1
                    ? key.GetValue(1) / ABObjectImporter.OpacityScale
                    : 1f;
                samples.Add(new OpacitySample(ABTimeMap.ToFrame(key.Time, framerate), opacity));
            }

            samples.Sort((a, b) => a.Frame.CompareTo(b.Frame));
            return samples;
        }

        // Anchor-stretched over the parent's whole rect and sized to nothing of its own, which is
        // what makes the child's rect identical to the parent's whatever the parent's own pivot,
        // size or anchors are (RectTransform2D.Apply adds `parent.size * (anchorMax - anchorMin)`).
        // Position, scale and rotation are left with no keyframes at all - an empty track is the
        // engine's own default, not missing data.
        private static ShapeObject BuildColliderChild(ShapeObject parent, ShapeId colliderId,
            FrameRange range, ABImportContext context)
        {
            var child = new ShapeObject
            {
                ObjectId = context.Mint(null),
                ParentObjectId = parent.ObjectId,
                Name = string.Empty,
                Active = true,
                Layer = 0,
                Span = new FrameSpan(parent.Span.StartFrame + range.Start, range.Duration),
                ShapeId = ShapeId.Null,
                ColliderId = colliderId,
                ShaderType = ShaderType.Auto,
            };

            child.AnchorsMin.Add(new AlignmentKey(Vector2Value.Zero, FrameRules.MinFrame));
            child.AnchorsMax.Add(new AlignmentKey(Vector2Value.One, FrameRules.MinFrame));
            child.Sizes.Add(new ScaKey(Vector2Value.Zero, FrameRules.MinFrame));

            return child;
        }
    }
}
