using System;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;

namespace BH.SDK.Interop.AfterBeat
{
    // Afterbeat randomizes a keyframe by carrying a type plus three spare numbers (Random X,
    // Random Y, Interval) beside the value; this format randomizes it by storing a different KIND
    // of value. So the conversion is not a field copy, it is a choice of variant:
    //
    //   None     -> the literal value.
    //   Linear   -> a range from the value to value+random. Exact.
    //   Toggle   -> the same range with the step set to its own width, which makes the only two
    //               values it can produce the two ends - exactly what a toggle is. Also exact, and
    //               the reason RandomMinMaxStep is worth reaching for here.
    //   Relative -> adds to the PREVIOUS keyframe's roll, which is a stateful generator. This
    //               format resolves randomness by address and has no previous roll to add to, so
    //               this one cannot cross and is reported.
    //
    // An Interval of its own turns any of the above into the step variant, which is the same
    // meaning in both formats: land on multiples rather than anywhere in the range.
    //
    // Rotation is the other thing this file owns, and it is the single most dangerous conversion in
    // the whole importer. Afterbeat writes rotation in DEGREES and each keyframe is RELATIVE to the
    // one before it; this format writes RADIANS and each keyframe is absolute. Getting either half
    // wrong produces a level that loads, plays, and spins.

    /// <summary> Afterbeat's keyframe values and randomization, mapped onto this format's
    /// polymorphic value types. </summary>
    public static class ABValueMap
    {
        /// <summary> Degrees to radians. The SDK cannot reach UnityEngine.Mathf. </summary>
        public const float DegreesToRadians = (float)(Math.PI / 180.0);

        /// <summary> Radians to degrees. </summary>
        public const float RadiansToDegrees = (float)(180.0 / Math.PI);

        /// <summary> Below this an interval is no interval at all - a step of zero would make every
        /// value in the range land on the range's own start. </summary>
        public const float MinInterval = 1e-4f;

        #region Floats

        /// <summary> One authored number, with whatever randomization the keyframe carried. </summary>
        public static IFloat ImportFloat(float value, VgdKeyframe source,
            InteropReport report = null, string path = null)
        {
            if (source == null) return new FloatValue(value);

            var random = source.GetRandom(0);
            var interval = source.GetRandom(2);

            switch ((ABRandomType)source.RandomType)
            {
                case ABRandomType.None:
                    return new FloatValue(value);

                case ABRandomType.Linear:
                    return MakeRange(value, value + random, interval);

                case ABRandomType.Toggle:
                    // A step equal to the whole width leaves exactly the two ends reachable.
                    return new FloatMinMaxStep(Min(value, value + random), Max(value, value + random),
                        Math.Max(Math.Abs(random), MinInterval));

                case ABRandomType.Relative:
                    report?.Dropped("random_relative",
                        "Afterbeat's Relative randomization adds to the previous keyframe's own roll. This format addresses randomness instead of accumulating it, so those keyframes use their authored value with no randomness.",
                        path);
                    return new FloatValue(value);

                default:
                    report?.Dropped("random_unknown",
                        $"Randomization type {source.RandomType} is not one this converter knows; those keyframes use their authored value.",
                        path);
                    return new FloatValue(value);
            }
        }

        private static IFloat MakeRange(float a, float b, float interval)
        {
            var min = Min(a, b);
            var max = Max(a, b);
            if (Math.Abs(max - min) < MinInterval) return new FloatValue(a);
            return interval > MinInterval
                ? new FloatMinMaxStep(min, max, interval)
                : new FloatMinMax(min, max);
        }

        #endregion

        #region Vectors

        /// <summary> One authored point, with whatever randomization the keyframe carried. </summary>
        public static IVector2 ImportVector(float x, float y, VgdKeyframe source,
            InteropReport report = null, string path = null)
        {
            if (source == null) return new Vector2Value(x, y);

            var randomX = source.GetRandom(0);
            var randomY = source.GetRandom(1);
            var interval = source.GetRandom(2);

            switch ((ABRandomType)source.RandomType)
            {
                case ABRandomType.None:
                    return new Vector2Value(x, y);

                case ABRandomType.Linear:
                    return MakeRect(x, y, x + randomX, y + randomY, interval);

                case ABRandomType.Toggle:
                {
                    var step = Math.Max(Math.Max(Math.Abs(randomX), Math.Abs(randomY)), MinInterval);
                    return new Vector2RectStep(Min(x, x + randomX), Min(y, y + randomY),
                        Max(x, x + randomX), Max(y, y + randomY), step);
                }

                case ABRandomType.Relative:
                    report?.Dropped("random_relative",
                        "Afterbeat's Relative randomization adds to the previous keyframe's own roll. This format addresses randomness instead of accumulating it, so those keyframes use their authored value with no randomness.",
                        path);
                    return new Vector2Value(x, y);

                default:
                    report?.Dropped("random_unknown",
                        $"Randomization type {source.RandomType} is not one this converter knows; those keyframes use their authored value.",
                        path);
                    return new Vector2Value(x, y);
            }
        }

        private static IVector2 MakeRect(float minX, float minY, float maxX, float maxY, float interval)
        {
            var x0 = Min(minX, maxX);
            var y0 = Min(minY, maxY);
            var x1 = Max(minX, maxX);
            var y1 = Max(minY, maxY);

            if (Math.Abs(x1 - x0) < MinInterval && Math.Abs(y1 - y0) < MinInterval)
                return new Vector2Value(x0, y0);

            return interval > MinInterval
                ? new Vector2RectStep(x0, y0, x1, y1, interval)
                : new Vector2Rect(x0, y0, x1, y1);
        }

        #endregion

        #region Export

        /// <summary> The single number a polymorphic float resolves to on the way out. A range has
        /// no representation in a plain Afterbeat keyframe, so it collapses to its midpoint. </summary>
        public static float ExportFloat(IFloat value, InteropReport report = null, string path = null)
        {
            switch (value)
            {
                case null:
                    return 0f;
                case FloatValue literal:
                    return literal.Value;
                case FloatMinMaxStep step:
                    ReportRandom(report, path);
                    return (step.Min + step.Max) * 0.5f;
                case FloatMinMax range:
                    ReportRandom(report, path);
                    return (range.Min + range.Max) * 0.5f;
                default:
                    ReportRandom(report, path);
                    return 0f;
            }
        }

        /// <summary> The same for a point. </summary>
        public static (float X, float Y) ExportVector(IVector2 value,
            InteropReport report = null, string path = null)
        {
            switch (value)
            {
                case null:
                    return (0f, 0f);
                case Vector2Value literal:
                    return (literal.X, literal.Y);
                case Vector2RectStep step:
                    ReportRandom(report, path);
                    return ((step.MinX + step.MaxX) * 0.5f, (step.MinY + step.MaxY) * 0.5f);
                case Vector2Rect rect:
                    ReportRandom(report, path);
                    return ((rect.MinX + rect.MaxX) * 0.5f, (rect.MinY + rect.MaxY) * 0.5f);
                case Vector2Circle circle:
                    ReportRandom(report, path);
                    return (circle.X, circle.Y);
                default:
                    ReportRandom(report, path);
                    return (0f, 0f);
            }
        }

        private static void ReportRandom(InteropReport report, string path)
            => report?.Approximated("random_resolved",
                "Afterbeat keyframes cannot carry this format's random ranges; those values export as the middle of their range.",
                path);

        #endregion

        #region Rotation

        /// <summary>
        /// Turns Afterbeat's chain of relative degree deltas into this format's absolute radians.
        /// Call once per track, in keyframe order, carrying <paramref name="accumulatedDegrees"/>
        /// between calls.
        /// </summary>
        public static float AccumulateRotation(float deltaDegrees, ref float accumulatedDegrees)
        {
            accumulatedDegrees += deltaDegrees;
            return accumulatedDegrees * DegreesToRadians;
        }

        /// <summary>
        /// The reverse - absolute radians back to the delta in degrees Afterbeat expects. Carry
        /// <paramref name="previousRadians"/> between calls; it starts at zero, because an object's
        /// first rotation keyframe is a delta from no rotation at all.
        /// </summary>
        public static float DifferentiateRotation(float absoluteRadians, ref float previousRadians)
        {
            var delta = absoluteRadians - previousRadians;
            previousRadians = absoluteRadians;
            return delta * RadiansToDegrees;
        }

        #endregion

        private static float Min(float a, float b) => a <= b ? a : b;
        private static float Max(float a, float b) => a >= b ? a : b;
    }
}
