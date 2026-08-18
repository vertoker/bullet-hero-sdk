using System;
using BH.SDK.Models.Enums;
using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat
{
    // Every scale below was READ OUT OF THE SOURCE GAME, not inferred from an inspector range, and
    // the two disagree more often than they agree. Afterbeat's own EventManager.Init*Events is the
    // only authority: it takes each keyframe component, optionally remaps it, and hands the result
    // to LSEffectsManager, which writes it straight into a URP VolumeComponent - the same
    // VolumeComponents this format's own post-processing describes. So wherever Afterbeat does NOT
    // remap, the number it writes IS a URP value and crosses untouched; wherever it does, that
    // remap is the whole conversion and nothing else may be invented on top of it.
    //
    // This replaced a transcription (AB-POST-PROCESSING-SPECIFICATION.md, still next to this folder
    // as the record of what was believed) whose ranges came from Project Arrhythmia's inspector
    // rather than from the game: bloom intensity 0-50, vignette intensity 0-100, vignette
    // smoothness halved, bloom diffusion divided rather than remapped. None of the four is what the
    // game does, and each was a level that renders at the wrong strength with nothing to notice.
    //
    // Both directions live side by side on purpose: an export that does not undo exactly what the
    // import did turns a round trip into a slow drift, and these are the numbers most likely to be
    // round-tripped by somebody moving one level between the two editors.
    //
    // The one number with no formula is the film grain TYPE, which is an enum in both formats and
    // an index in the file - see GrainTypeOffset.

    /// <summary> The scale each post-processing value crosses on, both directions. </summary>
    public static class ABPostProcessingMap
    {
        #region Bloom

        // LSEffectsManager.UpdateBloom writes _intensity into bloom.intensity with no remap at all,
        // so an Afterbeat bloom intensity is already a URP one. The old /5 made every converted
        // level's bloom five times too weak.

        public static float ImportBloomIntensity(float intensity)
            => Clamp(intensity,
                PostProcessingRules.Bloom.IntensityMin, PostProcessingRules.Bloom.IntensityMax);

        public static float ExportBloomIntensity(float intensity) => intensity;

        /// <summary> Afterbeat bloom diffusion runs 5-30 and is remapped onto URP's 0-1 scatter -
        /// EventManager.InitBloomEvents does LSMath.Remap(ev[1], 5, 30, 0, 1). </summary>
        public const float BloomDiffusionMin = 5f;
        public const float BloomDiffusionMax = 30f;

        /// <summary> What Afterbeat reads when a bloom keyframe writes no diffusion - the literal
        /// default of its own GetVal(1, 7f), i.e. a very tight bloom rather than a wide one. </summary>
        public const float DefaultBloomDiffusion = 7f;

        public static float ImportBloomScatter(float diffusion)
            => Clamp(Remap(diffusion, BloomDiffusionMin, BloomDiffusionMax, 0f, 1f),
                PostProcessingRules.Bloom.ScatterMin, PostProcessingRules.Bloom.ScatterMax);

        public static float ExportBloomScatter(float scatter)
            => Clamp(Remap(scatter, 0f, 1f, BloomDiffusionMin, BloomDiffusionMax),
                BloomDiffusionMin, BloomDiffusionMax);

        #endregion

        #region Chromatic aberration

        // EventManager.InitChromaEvents does LSMath.Remap(ev[0], 0, 8, 0, 3) before writing
        // chroma.intensity, and URP's own chromatic intensity is 0-1 - so the source range
        // SATURATES at 8/3, and the top two thirds of Afterbeat's own slider all look the same over
        // there. Reproducing the saturation is the point: dividing by 8 instead would render a
        // level authored at 3 as a third of the aberration its author saw.

        /// <summary> Afterbeat chromatic intensity runs 0-8, remapped onto 0-3 before it reaches a
        /// volume whose own range is 0-1. </summary>
        public const float ChromaticSourceMax = 8f;
        public const float ChromaticTargetMax = 3f;

        public static float ImportChromatic(float intensity)
            => Clamp(Remap(intensity, 0f, ChromaticSourceMax, 0f, ChromaticTargetMax),
                PostProcessingRules.ChromaticAberration.IntensityMin,
                PostProcessingRules.ChromaticAberration.IntensityMax);

        public static float ExportChromatic(float intensity)
            => Clamp(Remap(intensity, 0f, ChromaticTargetMax, 0f, ChromaticSourceMax),
                0f, ChromaticSourceMax);

        #endregion

        #region Vignette

        // Intensity, smoothness and centre all reach vignette.* unremapped
        // (LSEffectsManager.UpdateVignette), so all three are URP values already. The old /100 and
        // /2 made a converted vignette invisible.

        /// <summary> What Afterbeat substitutes for a smoothness of exactly zero - URP's own
        /// minimum, since a zero smoothness is a hard edge nothing authored. </summary>
        public const float VignetteSmoothnessFloor = 0.01f;

        public static float ImportVignetteIntensity(float intensity)
            => Clamp(intensity,
                PostProcessingRules.Vignette.IntensityMin, PostProcessingRules.Vignette.IntensityMax);

        public static float ExportVignetteIntensity(float intensity) => intensity;

        public static float ImportVignetteSmoothness(float smoothness)
            => Clamp(smoothness == 0f ? VignetteSmoothnessFloor : smoothness,
                PostProcessingRules.Vignette.SmoothnessMin, PostProcessingRules.Vignette.SmoothnessMax);

        public static float ExportVignetteSmoothness(float smoothness) => smoothness;

        public static float ImportVignetteCenter(float center)
            => Clamp(center, PostProcessingRules.Vignette.CenterMin, PostProcessingRules.Vignette.CenterMax);

        public static float ExportVignetteCenter(float center) => center;

        #endregion

        #region Lens distortion

        /// <summary> Afterbeat lens intensity is -80..80 against this format's -1..1 -
        /// EventManager.InitLensEvents does LSMath.Remap(ev[0], -80, 80, -1, 1). </summary>
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

        // A grain keyframe is [Intensity, unused, Type, Response]. Slot 2 is NOT a size - Afterbeat
        // casts it to UnityEngine.Rendering.Universal.FilmGrainLookup and clamps it to 0-9, i.e. it
        // is the same preset table this format's FilmGrainType is, only starting one earlier
        // because ours reserves 0 for None. Slot 1 is read into a field the effect never uses.

        /// <summary> Afterbeat's grain index 0 is this format's <see cref="FilmGrainType.Thin1"/>,
        /// since this format reserves 0 for "no grain". </summary>
        public const int GrainTypeOffset = 1;

        /// <summary> Lowest index Afterbeat's own clamp allows. </summary>
        public const int MinSourceGrainType = 0;

        /// <summary> Highest index Afterbeat's own clamp allows - FilmGrainLookup.Large02. </summary>
        public const int MaxSourceGrainType = 9;

        public static float ImportGrainIntensity(float intensity)
            => Clamp(intensity,
                PostProcessingRules.FilmGrain.IntensityMin, PostProcessingRules.FilmGrain.IntensityMax);

        public static float ExportGrainIntensity(float intensity) => intensity;

        public static FilmGrainType ImportGrainType(float sourceIndex)
        {
            var index = (int)Math.Clamp(sourceIndex, MinSourceGrainType, MaxSourceGrainType);
            return (FilmGrainType)(index + GrainTypeOffset);
        }

        public static float ExportGrainType(FilmGrainType type)
            => Math.Clamp((int)type - GrainTypeOffset, MinSourceGrainType, MaxSourceGrainType);

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

        // The source game's own LSMath.Remap, spelled out because this library has no engine to
        // borrow it from. Unclamped on purpose - every caller clamps into ITS OWN range afterwards,
        // and clamping here as well would hide which of the two ranges an out-of-range value hit.
        private static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            var span = fromMax - fromMin;
            if (Math.Abs(span) < float.Epsilon) return toMin;
            return toMin + (value - fromMin) / span * (toMax - toMin);
        }

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
