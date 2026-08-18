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

        // AFTERBEAT KEEPS NO TIME IT WAS GIVEN. Every keyframe time it reads or writes goes through
        // EventKeyframe.Time, whose setter converts to an integer count of hundredths of a second
        // and back (KeyframeOffsetFromTime / TimeFromKeyframeOffset), clamping a negative to zero
        // on the way. So the file's own grid is 10 ms, whatever this format's framerate is, and an
        // exported time that is not on it is silently moved to the nearest point that is.
        //
        // Snapping here rather than letting it happen over there is what makes an export honest
        // about what it wrote: a round trip returns the time the target game actually holds, and a
        // framerate whose frames are FINER than the grid is caught (see MaxLosslessFramerate)
        // instead of quietly merging two keyframes into one.

        /// <summary> The grid every time in a .vgd sits on, in seconds. </summary>
        public const float SourceTimeStep = 0.01f;

        /// <summary> The highest framerate whose frames stay distinct on that grid. Above it two
        /// frames can land on one source time, and the source format allows a track only one
        /// keyframe per time. </summary>
        public const int MaxLosslessFramerate = 100;

        /// <summary> A frame's own start, in seconds, on the grid the target format stores. </summary>
        public static float ToSeconds(int frame, int framerate)
        {
            if (framerate <= 0) framerate = FrameRules.MinFramerate;
            return SnapToSourceGrid(frame / (float)framerate);
        }

        /// <summary> One time as the target format will hold it. </summary>
        public static float SnapToSourceGrid(float seconds)
        {
            if (seconds <= 0f) return 0f;
            return (float)(Math.Round(seconds / (double)SourceTimeStep, MidpointRounding.AwayFromZero)
                           * SourceTimeStep);
        }

        // A TRACK OF ONE KEYFRAME CONTRIBUTES NOTHING, which is the source game's own rule rather
        // than an optimisation: BeatmapObject.GetLongestSequence skips any track whose keyframe
        // count is 1 or 0 before taking its maximum. A single keyframe is a constant, not an
        // animation, so an object holding one colour from second five onwards does not live until
        // second five - it dies immediately, exactly as it does over there. Taking the maximum over
        // every keyframe instead gives such an object a lifetime the source level never gave it.

        /// <summary> The last keyframe time across all four of an object's tracks, in seconds
        /// relative to the object's own start, counting only tracks that actually ANIMATE. Zero
        /// when it has none. </summary>
        public static float GetLastKeyframeTime(VgdObject source)
        {
            var last = 0f;
            if (source?.Tracks == null) return last;

            foreach (var track in source.Tracks)
            {
                if (track?.Keyframes == null || track.Keyframes.Count <= 1) continue;
                foreach (var keyframe in track.Keyframes)
                    if (keyframe != null && keyframe.Time > last) last = keyframe.Time;
            }
            return last;
        }

        /// <summary> What the source game gives an object whose Song Time autokill has already
        /// passed by the time it spawns - a tenth of a second, not a negative lifetime. </summary>
        public const float MinSongTimeLife = 0.1f;

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
                // Nothing to report: the source game resolves it exactly like Last Keyframe
                // wherever a level plays - see ABAutokillType.OldStyleNoAutokill.
                case ABAutokillType.OldStyleNoAutokill:
                case ABAutokillType.LastKeyframe:
                    return start + lastKey;
                case ABAutokillType.LastKeyframeOffset:
                    return start + lastKey + source.AutokillOffset;
                case ABAutokillType.FixedTime:
                    return start + source.AutokillOffset;

                // Song Time is the only rule that can name an end BEFORE the object's own start,
                // and the source game does not read that as a backwards lifetime - it substitutes a
                // tenth of a second. Resolving it as the raw offset instead builds a span running
                // from the offset to the start, i.e. an object that plays for the whole stretch of
                // level it was authored to be absent from.
                case ABAutokillType.SongTime:
                    return start >= source.AutokillOffset
                        ? start + MinSongTimeLife
                        : source.AutokillOffset;

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
