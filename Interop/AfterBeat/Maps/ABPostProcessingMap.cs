using System;
using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat
{
    // Every post-processing number crosses on a DIFFERENT scale, and none of the scales is written
    // anywhere in the documents themselves. Afterbeat inherited Project Arrhythmia's own ranges,
    // which were picked to be comfortable in an inspector (bloom 0-50, vignette 0-100, lens
    // -80..80); this project's are the URP volume's own (0-10, 0-1, -1..1). The two agree on what
    // each effect MEANS and on nothing else.
    //
    // Passing the numbers through untouched is therefore not a "close enough" - it is a level that
    // renders wrong in a way no report can catch, because every value out of range simply clamps at
    // the far end: a lens distortion authored at 30 arrives as a permanent maximum fisheye, and a
    // bloom scatter of 7 is not even a legal value for the model to hold.
    //
    // The formulas are the author's own (AB-POST-PROCESSING-SPECIFICATION.md, next to this folder),
    // with two decisions taken here:
    //
    //   bloom scatter is DIRECT (diffusion / 30), not the specification's reciprocal. The
    //   reciprocal answers 6 at the low end of the source range and 1 at the high end, so after
    //   clamping into [0, 1] every legal input becomes the same maximum scatter and the parameter
    //   stops existing.
    //
    //   vignette smoothness keeps the specification's own /2, which does saturate above a source
    //   value of 2 - deliberately, since that is where Project Arrhythmia's own smoothness stops
    //   being visually distinguishable.
    //
    // Both directions live side by side on purpose: an export that does not undo exactly what the
    // import did turns a round trip into a slow drift, and these are the numbers most likely to be
    // round-tripped by somebody moving one level between the two editors.

    /// <summary> The scale each post-processing value crosses on, both directions. </summary>
    public static class ABPostProcessingMap
    {
        #region Bloom

        /// <summary> Afterbeat bloom intensity is 0-50 against this format's 0-10. </summary>
        public const float BloomIntensityScale = 5f;

        /// <summary> Afterbeat bloom diffusion runs 5-30; scatter runs 0-1. </summary>
        public const float BloomDiffusionMin = 5f;
        public const float BloomDiffusionMax = 30f;

        public static float ImportBloomIntensity(float intensity)
            => Clamp(intensity / BloomIntensityScale,
                PostProcessingRules.Bloom.IntensityMin, PostProcessingRules.Bloom.IntensityMax);

        public static float ExportBloomIntensity(float intensity) => intensity * BloomIntensityScale;

        public static float ImportBloomScatter(float diffusion)
            => Clamp(Math.Max(diffusion, BloomDiffusionMin) / BloomDiffusionMax,
                PostProcessingRules.Bloom.ScatterMin, PostProcessingRules.Bloom.ScatterMax);

        public static float ExportBloomScatter(float scatter)
            => Clamp(scatter * BloomDiffusionMax, BloomDiffusionMin, BloomDiffusionMax);

        #endregion

        #region Chromatic aberration

        /// <summary> Afterbeat chromatic intensity is 0-8 against this format's 0-1. </summary>
        public const float ChromaticIntensityScale = 8f;

        public static float ImportChromatic(float intensity)
            => Clamp(intensity / ChromaticIntensityScale,
                PostProcessingRules.ChromaticAberration.IntensityMin,
                PostProcessingRules.ChromaticAberration.IntensityMax);

        public static float ExportChromatic(float intensity) => intensity * ChromaticIntensityScale;

        #endregion

        #region Vignette

        /// <summary> Afterbeat vignette intensity is 0-100 against this format's 0-1. </summary>
        public const float VignetteIntensityScale = 100f;

        /// <summary> Afterbeat vignette smoothness is -25..25; halving it lands the usable part of
        /// that range on this format's 0.01-1 and saturates the rest. </summary>
        public const float VignetteSmoothnessScale = 2f;

        /// <summary> Smallest smoothness the source range is read as, below this format's own
        /// minimum so a source zero does not read as a hard edge. </summary>
        public const float VignetteSmoothnessFloor = 0.02f;

        public static float ImportVignetteIntensity(float intensity)
            => Clamp(intensity / VignetteIntensityScale,
                PostProcessingRules.Vignette.IntensityMin, PostProcessingRules.Vignette.IntensityMax);

        public static float ExportVignetteIntensity(float intensity) => intensity * VignetteIntensityScale;

        public static float ImportVignetteSmoothness(float smoothness)
            => Clamp(Math.Max(smoothness, VignetteSmoothnessFloor) / VignetteSmoothnessScale,
                PostProcessingRules.Vignette.SmoothnessMin, PostProcessingRules.Vignette.SmoothnessMax);

        public static float ExportVignetteSmoothness(float smoothness)
            => smoothness * VignetteSmoothnessScale;

        public static float ImportVignetteCenter(float center)
            => Clamp(center, PostProcessingRules.Vignette.CenterMin, PostProcessingRules.Vignette.CenterMax);

        #endregion

        #region Lens distortion

        /// <summary> Afterbeat lens intensity is -80..80 against this format's -1..1. </summary>
        public const float LensIntensityScale = 80f;

        /// <summary> Afterbeat measures the lens centre from the middle of the screen (-0.5..0.5);
        /// this format measures it from the corner (0-1). </summary>
        public const float LensCenterOffset = 0.5f;

        public static float ImportLensIntensity(float intensity)
            => Clamp(intensity / LensIntensityScale,
                PostProcessingRules.LensDistortion.IntensityMin,
                PostProcessingRules.LensDistortion.IntensityMax);

        public static float ExportLensIntensity(float intensity) => intensity * LensIntensityScale;

        public static float ImportLensCenter(float center)
            => Clamp(center + LensCenterOffset,
                PostProcessingRules.LensDistortion.CenterMin, PostProcessingRules.LensDistortion.CenterMax);

        public static float ExportLensCenter(float center) => center - LensCenterOffset;

        #endregion

        #region Film grain and glitch

        public static float ImportGrainIntensity(float intensity)
            => Clamp(intensity,
                PostProcessingRules.FilmGrain.IntensityMin, PostProcessingRules.FilmGrain.IntensityMax);

        public static float ExportGrainIntensity(float intensity) => intensity;

        public static float ImportGlitchIntensity(float intensity)
            => Clamp(intensity,
                PostProcessingRules.DigitalGlitch.IntensityMin, PostProcessingRules.DigitalGlitch.IntensityMax);

        public static float ExportGlitchIntensity(float intensity) => intensity;

        #endregion

        #region Hue

        // Afterbeat's hue track is a global rotation in degrees; this format has no hue effect but
        // its colour curves carry a Hue vs Hue control whose midpoint (0.5) is "unchanged". So a
        // rotation of zero must land exactly on 0.5, not on 0, or importing a level that never
        // touched its hue would rotate every colour in it.

        /// <summary> A full turn of Afterbeat's hue track. </summary>
        public const float HueDegrees = 360f;

        /// <summary> Where "no rotation" sits on this format's Hue vs Hue control. </summary>
        public const float HueNeutral = 0.5f;

        /// <summary> Saturation is not part of the source track; the control's own midpoint leaves
        /// it alone. </summary>
        public const float SaturationNeutral = 0.5f;

        public static float ImportHue(float degrees)
            => Clamp(Repeat(degrees / HueDegrees + HueNeutral, 1f),
                PostProcessingRules.ColorCurves.HueVsHueMin, PostProcessingRules.ColorCurves.HueVsHueMax);

        public static float ExportHue(float hueVsHue)
            => Repeat(hueVsHue - HueNeutral, 1f) * HueDegrees;

        #endregion

        private static float Clamp(float value, float min, float max)
            => value < min ? min : value > max ? max : value;

        // Mathf lives in the engine and this library has none, so the one function borrowed from it
        // is spelled out. Unlike the % operator it answers a positive value for a negative input,
        // which is the whole reason a hue wraps rather than mirrors.
        private static float Repeat(float value, float length)
        {
            if (length <= 0f) return 0f;
            var wrapped = value - (float)Math.Floor(value / length) * length;
            return wrapped < 0f ? wrapped + length : wrapped;
        }
    }
}
