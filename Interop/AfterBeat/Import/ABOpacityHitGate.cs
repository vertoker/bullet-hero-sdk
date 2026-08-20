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
    /// WHERE THE BOUNDARY IS: at the alpha the object is actually DRAWN with on each frame, easing
    /// included, and not at the pair of keyframes bounding a segment. The source game re-reads the
    /// material every damage check, so the curve between two keys decides as much as the keys do -
    /// Instant holds the old opacity across its whole segment, an overshooting curve passes through
    /// 1 on its way somewhere below it, and a gentle fade is at full alpha for exactly as long as
    /// its own shape says. See <see cref="ResolveOpaqueRanges"/> for the frame-cell rule that keeps
    /// this from ever arming a collider the player already saw start to fade.
    ///
    /// WHERE THE AUTHOR CAN OVERRULE IT: <see cref="ABOptions.OpacityHitThreshold"/> is the alpha
    /// this pass arms a collider at, and 1 - the default - is the source game's own. Lowering it
    /// widens every window to whatever stretch is drawn at or above it, and zero switches the pass
    /// off entirely, leaving each object holding its own collider for its whole life. That is a
    /// deliberate break with the source level rather than a tuning knob, and the report says so.
    ///
    /// WHAT IT DELIBERATELY DOES NOT DO: an object opaque for its whole life is left untouched, which
    /// is the overwhelming majority of every level, so nothing pays for this but the levels that use
    /// it. And the boundary is read from the SOURCE's own opacity percentages rather than from the
    /// imported alpha: the rule is defined over there in percent against 100, and re-deriving it from
    /// a colour that may have become a theme reference would be answering a different question.
    /// </summary>
    public static class ABOpacityHitGate
    {
        /// <summary> One source opacity keyframe, reduced to the three things this rule reads. </summary>
        public readonly struct OpacitySample
        {
            public readonly int Frame;
            public readonly float Opacity;

            /// <summary> The curve this sample is reached BY. Afterbeat stores easing on the
            /// keyframe being interpolated TOWARDS, same as this format does, so a track's first
            /// sample never uses its own. </summary>
            public readonly EaseType Ease;

            public OpacitySample(int frame, float opacity) : this(frame, opacity, EaseType.Linear) { }

            public OpacitySample(int frame, float opacity, EaseType ease)
            {
                Frame = frame;
                Opacity = opacity;
                Ease = ease;
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

        // Alpha is compared against the threshold with no epsilon, exactly as the source game
        // compares it against 1: the value there is opacity/100 of an integer percentage, so every
        // legal value is either on a percent boundary or visibly off one, and a tolerance would only
        // invent a band where the two implementations disagree. Interpolating between two such
        // values does not change that - where both ends are 1 the lerp multiplies a zero difference
        // and returns exactly 1, and anywhere else the curve walks between values a percent apart.
        //
        // The threshold itself is ABOptions.OpacityHitThreshold, and the default is the only value
        // that reproduces the source level; see that field for why it can be lowered at all.
        private const float DefaultThreshold = ABOptions.DefaultOpacityHitThreshold;

        /// <summary> Rewrites one imported object's collider to exist only while the source was fully
        /// opaque. Adds the extra collider children to the scope; a no-op for anything that carries
        /// no collider or never fades. </summary>
        public static void Apply(VgdObject source, RectObject target, ABImportContext context, string path)
        {
            if (source == null || context?.Scope?.Objects == null) return;
            if (target is not ShapeObject shape) return;
            if (!shape.ColliderId.IsEnabled()) return;

            var threshold = context.Options.OpacityHitThreshold;
            if (threshold <= 0f) return;

            // No colour track at all is not "invisible": the source game's own reader fills a
            // missing opacity with 100, the same call ABObjectImporter.OpacityOf makes.
            var samples = CollectSamples(source, context.Options.Framerate);
            if (samples.Count == 0) return;

            var duration = target.Span.FrameDuration;
            var ranges = ResolveOpaqueRanges(samples, duration, threshold);

            if (IsWholeSpan(ranges, duration)) return;

            var colliderId = shape.ColliderId;
            shape.ColliderId = ShapeId.Null;

            // Both reports say "in the source game" only at the default threshold - below it the
            // level is being deliberately made to differ, and a report claiming fidelity for a
            // choice the author made against it is worse than no report.
            var faithful = threshold >= DefaultThreshold;

            if (ranges.Count == 0)
            {
                context.Report.Approximated("collider_opacity_never_opaque",
                    faithful
                        ? "Objects that never reach full opacity cannot hurt the player in the source game, so they were imported without a collider."
                        : "Objects never drawn at or above the opacity threshold this import was given were imported without a collider.",
                    path);
                return;
            }

            foreach (var range in ranges)
            {
                var child = BuildColliderChild(shape, colliderId, range, context);
                context.Scope.Objects[child.ObjectId] = child;
            }

            context.Report.Approximated("collider_opacity_gate",
                faithful
                    ? "Objects only hurt the player at full opacity in the source game; a faded object was imported as a collider-less object plus one invisible collider per fully opaque stretch."
                    : "This import was given a lowered opacity threshold, so a faded object was imported as a collider-less object plus one invisible collider per stretch drawn at or above it - which is wider than the source game would hit for.",
                path);
        }

        // THE WINDOW IS RESOLVED FROM THE CURVE, not from the pair of keyframes bounding a segment.
        // The rule over there reads the alpha the object is CURRENTLY drawn with, so the shape of the
        // easing between two keys is part of the answer and not a detail of playback: Instant holds
        // the old opacity for the whole segment and drops on the last frame, an overshooting curve
        // (Back, Elastic) can pass through 1 on the way to a value below it, and both of those are
        // frames on which the source game does damage. Reading only the two ends called the first
        // decoration for its whole segment and the second harmless throughout.
        //
        // A FRAME IS A CELL, so both of its boundaries have to be opaque for it to count - the same
        // half-open convention FrameSpan uses, applied to the question "was the object at full alpha
        // for the whole time this frame was on screen". That is what keeps the fade stopping the
        // collider on the keyframe it begins from rather than one frame later: at most half a frame
        // earlier than the source game, in the direction that cannot kill a player who already saw
        // the object start to fade.

        /// <summary> The stretches of [0, duration) over which the source was fully opaque. </summary>
        public static List<FrameRange> ResolveOpaqueRanges(IReadOnlyList<OpacitySample> samples, int duration)
            => ResolveOpaqueRanges(samples, duration, DefaultThreshold);

        /// <summary> The stretches of [0, duration) the source spent drawn at or above
        /// <paramref name="threshold"/> alpha. </summary>
        public static List<FrameRange> ResolveOpaqueRanges(IReadOnlyList<OpacitySample> samples,
            int duration, float threshold)
        {
            var ranges = new List<FrameRange>();
            if (samples == null || samples.Count == 0 || duration <= 0) return ranges;

            // Walked forwards once, so the cursor only ever moves up: the whole sweep costs one pass
            // over the frames plus one over the keys, not a key search per frame.
            var cursor = 0;
            var start = -1;
            var opaqueAtOpeningEdge = IsOpaque(OpacityAt(samples, 0f, ref cursor), threshold);

            for (var frame = 0; frame < duration; frame++)
            {
                var opaqueAtClosingEdge = IsOpaque(OpacityAt(samples, frame + 1f, ref cursor), threshold);
                var opaque = opaqueAtOpeningEdge && opaqueAtClosingEdge;
                opaqueAtOpeningEdge = opaqueAtClosingEdge;

                if (opaque)
                {
                    if (start < 0) start = frame;
                    continue;
                }

                if (start < 0) continue;
                ranges.Add(new FrameRange(start, frame - start));
                start = -1;
            }

            if (start >= 0) ranges.Add(new FrameRange(start, duration - start));
            return ranges;
        }

        /// <summary> The source's own opacity at one point of an object's life, in frames local to
        /// its span. Fractional frames are legal and meaningful - a frame cell's two boundaries are
        /// what <see cref="ResolveOpaqueRanges"/> reads. Samples must be sorted by frame. </summary>
        public static float ResolveOpacity(IReadOnlyList<OpacitySample> samples, float frame)
        {
            var cursor = 0;
            return OpacityAt(samples, frame, ref cursor);
        }

        // Held before the first key and after the last one, exactly as the source game holds a
        // sequence's ends. Two keys on the same frame are a step rather than a division by zero:
        // the later one's value is what the object is drawn with from that frame on.
        private static float OpacityAt(IReadOnlyList<OpacitySample> samples, float frame, ref int cursor)
        {
            if (samples == null || samples.Count == 0) return DefaultThreshold;
            if (frame <= samples[0].Frame) return samples[0].Opacity;

            while (cursor + 1 < samples.Count && samples[cursor + 1].Frame <= frame) cursor++;
            if (cursor + 1 >= samples.Count) return samples[cursor].Opacity;

            var from = samples[cursor];
            var to = samples[cursor + 1];

            var span = to.Frame - from.Frame;
            if (span <= 0) return to.Opacity;

            var t = (frame - from.Frame) / span;
            return from.Opacity + (to.Opacity - from.Opacity) * ABEaseMap.Evaluate(to.Ease, t);
        }

        private static bool IsOpaque(float opacity, float threshold) => opacity >= threshold;

        private static bool IsWholeSpan(IReadOnlyList<FrameRange> ranges, int duration)
            => ranges.Count == 1 && ranges[0].Start == 0 && ranges[0].Duration >= duration;

        // The easing comes across unreported on purpose: ABObjectImporter reads the same names off
        // the same keys for the colour track itself and reports whatever it approximates there, and
        // a second pass over the same keyframes would only say it twice.
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
                samples.Add(new OpacitySample(ABTimeMap.ToFrame(key.Time, framerate), opacity,
                    ABEaseMap.Import(key.Ease)));
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
