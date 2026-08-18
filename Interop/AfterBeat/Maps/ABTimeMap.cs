using System;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat
{
    // Afterbeat measures time in SECONDS and this format measures it in FRAMES, so every crossing
    // is a rounding, and the framerate the level is imported at decides how coarse. Rounding is to
    // nearest rather than truncating: an object authored at 1.4999 seconds belongs on the frame it
    // is nearly on, not on the one before it.
    //
    // The two formats also disagree about what a lifetime IS. Afterbeat stores a start plus a rule
    // for when the object dies (autokill), where three of the four rules cannot be evaluated
    // without looking at the object's own keyframes. This format stores a half-open span. So the
    // conversion is not a unit change - it is a resolution, and it is the one place that knows how
    // to do it.

    /// <summary> Seconds to frames, and Afterbeat's autokill rules to a <see cref="FrameSpan"/>. </summary>
    public static class ABTimeMap
    {
        /// <summary> Seconds to the frame containing them. </summary>
        public static int ToFrame(float seconds, int framerate)
        {
            if (framerate <= 0) framerate = FrameRules.MinFramerate;
            var frame = (long)Math.Round((double)seconds * framerate, MidpointRounding.AwayFromZero);
            if (frame < FrameRules.MinFrame) return FrameRules.MinFrame;
            if (frame > FrameRules.MaxFrame) return FrameRules.MaxFrame;
            return (int)frame;
        }

        /// <summary> A frame's own start, in seconds. </summary>
        public static float ToSeconds(int frame, int framerate)
        {
            if (framerate <= 0) framerate = FrameRules.MinFramerate;
            return frame / (float)framerate;
        }

        /// <summary> The last keyframe time across all four of an object's tracks, in seconds
        /// relative to the object's own start. Zero when it has no keyframes at all. </summary>
        public static float GetLastKeyframeTime(VgdObject source)
        {
            var last = 0f;
            if (source?.Tracks == null) return last;

            foreach (var track in source.Tracks)
            {
                if (track?.Keyframes == null) continue;
                foreach (var keyframe in track.Keyframes)
                    if (keyframe != null && keyframe.Time > last) last = keyframe.Time;
            }
            return last;
        }

        /// <summary>
        /// Absolute end time in seconds, resolved from the object's autokill rule. An unknown rule
        /// is treated as Last Keyframe, which is the format's own default.
        /// </summary>
        public static float ResolveEndTime(VgdObject source, InteropReport report = null, string path = null)
        {
            if (source == null) return 0f;

            var start = source.StartTime;
            var lastKey = GetLastKeyframeTime(source);

            switch ((ABAutokillType)source.AutokillType)
            {
                case ABAutokillType.LastKeyframe:
                    return start + lastKey;
                case ABAutokillType.LastKeyframeOffset:
                    return start + lastKey + source.AutokillOffset;
                case ABAutokillType.FixedTime:
                    return start + source.AutokillOffset;
                case ABAutokillType.SongTime:
                    return source.AutokillOffset;
                default:
                    report?.Approximated("autokill_unknown",
                        $"Autokill type {source.AutokillType} is not one this converter knows; those objects die on their last keyframe.",
                        path);
                    return start + lastKey;
            }
        }

        /// <summary>
        /// The object's whole lifetime as a half-open span. Never shorter than one frame - a span
        /// cannot represent zero, and an object that lived for an instant in Afterbeat did exist.
        /// </summary>
        public static FrameSpan ResolveSpan(VgdObject source, int framerate,
            InteropReport report = null, string path = null)
        {
            if (source == null) return new FrameSpan(FrameRules.MinFrame, FrameRules.MinFrameDuration);

            var startFrame = ToFrame(source.StartTime, framerate);
            var endFrame = ToFrame(ResolveEndTime(source, report, path), framerate);

            return FromFrames(startFrame, endFrame);
        }

        /// <summary> Builds a legal span out of two frames in either order. </summary>
        public static FrameSpan FromFrames(int startFrame, int endFrame)
        {
            if (endFrame < startFrame) (startFrame, endFrame) = (endFrame, startFrame);

            var start = Math.Clamp(startFrame, FrameRules.MinFrame, FrameRules.MaxFrame);
            var duration = Math.Max(FrameRules.MinFrameDuration, endFrame - start);
            var maxDuration = FrameRules.MaxFrameDuration - start;
            if (duration > maxDuration) duration = Math.Max(FrameRules.MinFrameDuration, maxDuration);

            return new FrameSpan(start, duration);
        }

        /// <summary>
        /// Writes a span back as Afterbeat's start + Fixed Time autokill. Fixed Time is the only
        /// one of the four that expresses a span without depending on the keyframes inside it,
        /// which is what makes the export independent of what those keyframes end up as.
        /// </summary>
        public static void ExportSpan(FrameSpan span, int framerate, VgdObject target)
        {
            if (target == null) return;

            target.StartTime = ToSeconds(span.StartFrame, framerate);
            target.AutokillType = (int)ABAutokillType.FixedTime;
            target.AutokillOffset = ToSeconds(span.FrameDuration, framerate);
        }
    }
}
