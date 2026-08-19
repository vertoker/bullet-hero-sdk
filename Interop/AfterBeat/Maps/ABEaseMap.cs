using System;
using System.Collections.Generic;
using BH.SDK.Models.Enums;

namespace BH.SDK.Interop.AfterBeat
{
    // The two easing vocabularies overlap but neither contains the other, and both gaps are real:
    //
    //   Afterbeat has Bounce (in/out/inout), which this format does not have at all.
    //   This format has Cubic, Quart and Quint, which Afterbeat does not have at all.
    //
    // Neither gap can be closed by picking a better name, so both are approximations that get
    // reported. Bounce becomes Elastic because both overshoot and settle - Back overshoots once and
    // stops, which reads as a completely different motion. The polynomial families collapse onto
    // their nearest neighbour by exponent: Cubic to Quad (one power down, the only one available),
    // Quart and Quint to Expo (the steepest curve Afterbeat has).
    //
    // Afterbeat stores the NAME, this format stores a number, so the mapping is by string on one
    // side and by enum on the other. An unknown name is Linear - a keyframe with an unreadable
    // curve still has to land on its value at its own frame, and Linear is the only answer that is
    // never surprising.
    //
    // THE TABLE BELOW IS EXHAUSTIVE, and that is a measurement rather than a hope. The source game
    // reads a curve name through a dictionary INDEXER (LSAnimationConverter.ReadJson:
    // AnimationListDictionaryStr[text]), so a name it does not have is a KeyNotFoundException and a
    // level that will not load at all - which makes "which names exist" the highest-stakes question
    // on the export side, and one no .vgd can answer, since a level only proves the names it uses.
    // That dictionary is filled from an Inspector list, so it is not in the game's code either; it
    // is in its scene data (Afterbeat_Data/level2, around offset 48060), and reading the strings
    // there gives exactly these 23: Linear, Instant, and In/Out/InOut of Sine, Quad, Expo, Circ,
    // Back, Elastic and Bounce. No Cubic, Quart or Quint - the approximations below are the whole
    // gap and not a guess at one.

    /// <summary> Easing names as Afterbeat writes them, mapped onto <see cref="EaseType"/>. </summary>
    public static class ABEaseMap
    {
        /// <summary> What Afterbeat means by an absent "ct". </summary>
        public const string DefaultEaseName = "Linear";

        /// <summary> Afterbeat's name for a keyframe that does not interpolate at all. </summary>
        public const string InstantEaseName = "Instant";

        private static readonly Dictionary<string, EaseType> ToEase = new()
        {
            { DefaultEaseName, EaseType.Linear },
            { InstantEaseName, EaseType.Constant },

            { "InSine", EaseType.InSine },
            { "OutSine", EaseType.OutSine },
            { "InOutSine", EaseType.InOutSine },

            { "InQuad", EaseType.InQuad },
            { "OutQuad", EaseType.OutQuad },
            { "InOutQuad", EaseType.InOutQuad },

            { "InExpo", EaseType.InExpo },
            { "OutExpo", EaseType.OutExpo },
            { "InOutExpo", EaseType.InOutExpo },

            { "InCirc", EaseType.InCirc },
            { "OutCirc", EaseType.OutCirc },
            { "InOutCirc", EaseType.InOutCirc },

            { "InBack", EaseType.InBack },
            { "OutBack", EaseType.OutBack },
            { "InOutBack", EaseType.InOutBack },

            { "InElastic", EaseType.InElastic },
            { "OutElastic", EaseType.OutElastic },
            { "InOutElastic", EaseType.InOutElastic },

            // No Bounce here - see the header. These are the approximations.
            { "InBounce", EaseType.InElastic },
            { "OutBounce", EaseType.OutElastic },
            { "InOutBounce", EaseType.InOutElastic },
        };

        private static readonly Dictionary<EaseType, string> ToName = new()
        {
            { EaseType.Linear, DefaultEaseName },
            { EaseType.Constant, InstantEaseName },

            { EaseType.InSine, "InSine" },
            { EaseType.OutSine, "OutSine" },
            { EaseType.InOutSine, "InOutSine" },

            { EaseType.InQuad, "InQuad" },
            { EaseType.OutQuad, "OutQuad" },
            { EaseType.InOutQuad, "InOutQuad" },

            { EaseType.InExpo, "InExpo" },
            { EaseType.OutExpo, "OutExpo" },
            { EaseType.InOutExpo, "InOutExpo" },

            { EaseType.InCirc, "InCirc" },
            { EaseType.OutCirc, "OutCirc" },
            { EaseType.InOutCirc, "InOutCirc" },

            { EaseType.InBack, "InBack" },
            { EaseType.OutBack, "OutBack" },
            { EaseType.InOutBack, "InOutBack" },

            { EaseType.InElastic, "InElastic" },
            { EaseType.OutElastic, "OutElastic" },
            { EaseType.InOutElastic, "InOutElastic" },

            // Afterbeat has no polynomial family past Quad - see the header.
            { EaseType.InCubic, "InQuad" },
            { EaseType.OutCubic, "OutQuad" },
            { EaseType.InOutCubic, "InOutQuad" },
            { EaseType.InQuart, "InExpo" },
            { EaseType.OutQuart, "OutExpo" },
            { EaseType.InOutQuart, "InOutExpo" },
            { EaseType.InQuint, "InExpo" },
            { EaseType.OutQuint, "OutExpo" },
            { EaseType.InOutQuint, "InOutExpo" },
        };

        private static readonly HashSet<string> ApproximatedNames = new()
        {
            "InBounce", "OutBounce", "InOutBounce",
        };

        private static readonly HashSet<EaseType> ApproximatedEases = new()
        {
            EaseType.InCubic, EaseType.OutCubic, EaseType.InOutCubic,
            EaseType.InQuart, EaseType.OutQuart, EaseType.InOutQuart,
            EaseType.InQuint, EaseType.OutQuint, EaseType.InOutQuint,
        };

        /// <summary> Every name Afterbeat can write. </summary>
        public static IReadOnlyCollection<string> KnownNames => ToEase.Keys;

        /// <summary>
        /// Reads an Afterbeat easing name. Reports an approximation for a curve this format has no
        /// equal of, and a drop for a name nothing here recognises.
        /// </summary>
        public static EaseType Import(string name, InteropReport report = null, string path = null)
        {
            if (string.IsNullOrEmpty(name)) return EaseType.Linear;

            if (!ToEase.TryGetValue(name, out var ease))
            {
                report?.Approximated("ease_unknown",
                    $"Easing '{name}' is not one this format knows; those keyframes use Linear.", path);
                return EaseType.Linear;
            }

            if (ApproximatedNames.Contains(name))
                report?.Approximated("ease_bounce",
                    "Bounce easing has no equivalent here; those keyframes use Elastic, which is the closest motion available.",
                    path);

            return ease;
        }

        /// <summary>
        /// Writes an easing name Afterbeat accepts. Reports an approximation for the polynomial
        /// families it has no equal of.
        /// </summary>
        public static string Export(EaseType ease, InteropReport report = null, string path = null)
        {
            if (!ToName.TryGetValue(ease, out var name))
            {
                report?.Approximated("ease_unknown",
                    $"Easing '{ease}' has no Afterbeat name; those keyframes use Linear.", path);
                return DefaultEaseName;
            }

            if (ApproximatedEases.Contains(ease))
                report?.Approximated("ease_polynomial",
                    "Afterbeat has no Cubic/Quart/Quint easing; those keyframes use the nearest curve it does have.",
                    path);

            return name;
        }

        // THIS FORMAT NEVER EVALUATES AN EASE, and that is why the arithmetic below lives here
        // rather than in Utils. An EaseType is stored on a keyframe and resolved by whoever plays
        // the level - the Unity project's own Easings - and the SDK deliberately cannot reach that
        // way round. A particle curve is the one thing that has to be BAKED rather than stored: its
        // keys carry tangents and no EaseType at all, so the shape has to be sampled at conversion
        // time or lost. Only ABCurveMap needs this, which is why it sits in the interop folder
        // instead of being offered to the format at large.

        /// <summary> One easing evaluated at normalized progress, for baking a curve. </summary>
        public static float Evaluate(EaseType ease, float t)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 1f;

            switch (ease)
            {
                case EaseType.Linear: return t;
                case EaseType.Constant: return 0f;

                case EaseType.InSine: return 1f - (float)Math.Cos(t * Math.PI / 2d);
                case EaseType.OutSine: return (float)Math.Sin(t * Math.PI / 2d);
                case EaseType.InOutSine: return (float)(-(Math.Cos(Math.PI * t) - 1d) / 2d);

                case EaseType.InQuad: return t * t;
                case EaseType.OutQuad: return 1f - (1f - t) * (1f - t);
                case EaseType.InOutQuad: return t < 0.5f
                    ? 2f * t * t
                    : 1f - (float)Math.Pow(-2d * t + 2d, 2d) / 2f;

                case EaseType.InCubic: return t * t * t;
                case EaseType.OutCubic: return 1f - (float)Math.Pow(1d - t, 3d);
                case EaseType.InOutCubic: return t < 0.5f
                    ? 4f * t * t * t
                    : 1f - (float)Math.Pow(-2d * t + 2d, 3d) / 2f;

                case EaseType.InQuart: return t * t * t * t;
                case EaseType.OutQuart: return 1f - (float)Math.Pow(1d - t, 4d);
                case EaseType.InOutQuart: return t < 0.5f
                    ? 8f * t * t * t * t
                    : 1f - (float)Math.Pow(-2d * t + 2d, 4d) / 2f;

                case EaseType.InQuint: return t * t * t * t * t;
                case EaseType.OutQuint: return 1f - (float)Math.Pow(1d - t, 5d);
                case EaseType.InOutQuint: return t < 0.5f
                    ? 16f * t * t * t * t * t
                    : 1f - (float)Math.Pow(-2d * t + 2d, 5d) / 2f;

                case EaseType.InExpo: return (float)Math.Pow(2d, 10d * t - 10d);
                case EaseType.OutExpo: return 1f - (float)Math.Pow(2d, -10d * t);
                case EaseType.InOutExpo: return t < 0.5f
                    ? (float)Math.Pow(2d, 20d * t - 10d) / 2f
                    : (2f - (float)Math.Pow(2d, -20d * t + 10d)) / 2f;

                case EaseType.InCirc: return 1f - (float)Math.Sqrt(1d - t * (double)t);
                case EaseType.OutCirc: return (float)Math.Sqrt(1d - Math.Pow(t - 1d, 2d));
                case EaseType.InOutCirc: return t < 0.5f
                    ? (float)((1d - Math.Sqrt(1d - Math.Pow(2d * t, 2d))) / 2d)
                    : (float)((Math.Sqrt(1d - Math.Pow(-2d * t + 2d, 2d)) + 1d) / 2d);

                case EaseType.InBack: return (BackC3 * t - BackC1) * t * t;
                case EaseType.OutBack: return 1f + BackC3 * (float)Math.Pow(t - 1d, 3d)
                                              + BackC1 * (float)Math.Pow(t - 1d, 2d);
                case EaseType.InOutBack: return t < 0.5f
                    ? (float)(Math.Pow(2d * t, 2d) * ((BackC2 + 1d) * 2d * t - BackC2) / 2d)
                    : (float)((Math.Pow(2d * t - 2d, 2d) * ((BackC2 + 1d) * (t * 2d - 2d) + BackC2) + 2d) / 2d);

                case EaseType.InElastic:
                    return (float)(-Math.Pow(2d, 10d * t - 10d) * Math.Sin((t * 10d - 10.75d) * ElasticC4));
                case EaseType.OutElastic:
                    return (float)(Math.Pow(2d, -10d * t) * Math.Sin((t * 10d - 0.75d) * ElasticC4) + 1d);
                case EaseType.InOutElastic: return t < 0.5f
                    ? (float)(-(Math.Pow(2d, 20d * t - 10d) * Math.Sin((20d * t - 11.125d) * ElasticC5)) / 2d)
                    : (float)(Math.Pow(2d, -20d * t + 10d) * Math.Sin((20d * t - 11.125d) * ElasticC5) / 2d + 1d);

                default: return t;
            }
        }

        private const float BackC1 = 1.70158f;
        private const float BackC2 = BackC1 * 1.525f;
        private const float BackC3 = BackC1 + 1f;
        private const double ElasticC4 = 2d * Math.PI / 3d;
        private const double ElasticC5 = 2d * Math.PI / 4.5d;
    }
}
