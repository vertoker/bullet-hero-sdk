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
    }
}
