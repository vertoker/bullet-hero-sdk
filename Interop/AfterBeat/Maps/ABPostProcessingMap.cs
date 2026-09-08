using System;
using System.Collections.Generic;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Enums.Values;
using BH.SDK.Models.Values;
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

        /// <summary> Crosses untouched - the source writes this straight into URP - and is only clamped. </summary>
        public static float ImportBloomIntensity(float intensity)
            => Clamp(intensity,
                PostProcessingRules.Bloom.IntensityMin, PostProcessingRules.Bloom.IntensityMax);

        /// <summary> Back, unchanged. </summary>
        public static float ExportBloomIntensity(float intensity) => intensity;

        /// <summary> Afterbeat bloom diffusion runs 5-30 and is remapped onto URP's 0-1 scatter -
        /// EventManager.InitBloomEvents does LSMath.Remap(ev[1], 5, 30, 0, 1). </summary>
        public const float BloomDiffusionMin = 5f;
        /// <summary> Top of that diffusion range. </summary>
        public const float BloomDiffusionMax = 30f;

        /// <summary> What Afterbeat reads when a bloom keyframe writes no diffusion - the literal
        /// default of its own GetVal(1, 7f), i.e. a very tight bloom rather than a wide one. </summary>
        public const float DefaultBloomDiffusion = 7f;

        /// <summary> Diffusion 5-30 onto URP's 0-1 scatter. </summary>
        public static float ImportBloomScatter(float diffusion)
            => Clamp(Remap(diffusion, BloomDiffusionMin, BloomDiffusionMax, 0f, 1f),
                PostProcessingRules.Bloom.ScatterMin, PostProcessingRules.Bloom.ScatterMax);

        /// <summary> Scatter 0-1 back onto diffusion 5-30. </summary>
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
        /// <summary> Top of the remapped range, and above URP's own 1 - which is why the source saturates. </summary>
        public const float ChromaticTargetMax = 3f;

        /// <summary> Intensity 0-8 onto 0-3, saturation and all, then clamped to what this format allows. </summary>
        public static float ImportChromatic(float intensity)
            => Clamp(Remap(intensity, 0f, ChromaticSourceMax, 0f, ChromaticTargetMax),
                PostProcessingRules.ChromaticAberration.IntensityMin,
                PostProcessingRules.ChromaticAberration.IntensityMax);

        /// <summary> Back onto 0-8. Anything that saturated on the way in cannot be recovered. </summary>
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

        /// <summary> Crosses untouched; only clamped. </summary>
        public static float ImportVignetteIntensity(float intensity)
            => Clamp(intensity,
                PostProcessingRules.Vignette.IntensityMin, PostProcessingRules.Vignette.IntensityMax);

        /// <summary> Back, unchanged. </summary>
        public static float ExportVignetteIntensity(float intensity) => intensity;

        /// <summary> Crosses untouched, except that an exact zero becomes the floor the source itself substitutes. </summary>
        public static float ImportVignetteSmoothness(float smoothness)
            => Clamp(smoothness == 0f ? VignetteSmoothnessFloor : smoothness,
                PostProcessingRules.Vignette.SmoothnessMin, PostProcessingRules.Vignette.SmoothnessMax);

        /// <summary> Back, unchanged. </summary>
        public static float ExportVignetteSmoothness(float smoothness) => smoothness;

        /// <summary> Crosses untouched; only clamped. </summary>
        public static float ImportVignetteCenter(float center)
            => Clamp(center, PostProcessingRules.Vignette.CenterMin, PostProcessingRules.Vignette.CenterMax);

        /// <summary> Back, unchanged. </summary>
        public static float ExportVignetteCenter(float center) => center;

        #endregion

        #region Lens distortion

        /// <summary> Afterbeat lens intensity is -80..80 against this format's -1..1 -
        /// EventManager.InitLensEvents does LSMath.Remap(ev[0], -80, 80, -1, 1). </summary>
        public const float LensIntensityScale = 80f;

        /// <summary> Afterbeat measures the lens centre from the middle of the screen (-0.5..0.5);
        /// this format measures it from the corner (0-1). </summary>
        public const float LensCenterOffset = 0.5f;

        /// <summary> Divides the source's -80..80 down to this format's -1..1. </summary>
        public static float ImportLensIntensity(float intensity)
            => Clamp(intensity / LensIntensityScale,
                PostProcessingRules.LensDistortion.IntensityMin,
                PostProcessingRules.LensDistortion.IntensityMax);

        /// <summary> Multiplies back up. </summary>
        public static float ExportLensIntensity(float intensity) => intensity * LensIntensityScale;

        /// <summary> Shifts a centre measured from the middle of the screen to one measured from the corner. </summary>
        public static float ImportLensCenter(float center)
            => Clamp(center + LensCenterOffset,
                PostProcessingRules.LensDistortion.CenterMin, PostProcessingRules.LensDistortion.CenterMax);

        /// <summary> Shifts it back. </summary>
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

        /// <summary> Crosses untouched; only clamped. </summary>
        public static float ImportGrainIntensity(float intensity)
            => Clamp(intensity,
                PostProcessingRules.FilmGrain.IntensityMin, PostProcessingRules.FilmGrain.IntensityMax);

        /// <summary> Back, unchanged. </summary>
        public static float ExportGrainIntensity(float intensity) => intensity;

        /// <summary> The source's grain index as this format's enum - clamped to what the source itself allows,
        /// then shifted by one, since 0 here means no grain at all. </summary>
        public static FilmGrainType ImportGrainType(float sourceIndex)
        {
            var index = (int)Math.Clamp(sourceIndex, MinSourceGrainType, MaxSourceGrainType);
            return (FilmGrainType)(index + GrainTypeOffset);
        }

        /// <summary> Back to that index. <c>None</c> lands on the source's first grain, which has no counterpart. </summary>
        public static float ExportGrainType(FilmGrainType type)
            => Math.Clamp((int)type - GrainTypeOffset, MinSourceGrainType, MaxSourceGrainType);

        /// <summary> Crosses untouched; only clamped. </summary>
        public static float ImportGlitchIntensity(float intensity)
            => Clamp(intensity,
                PostProcessingRules.DigitalGlitch.IntensityMin, PostProcessingRules.DigitalGlitch.IntensityMax);

        /// <summary> Back, unchanged. </summary>
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

        /// <summary> A rotation in degrees as a point on the Hue vs Hue control, where 0.5 is "unchanged". </summary>
        public static float ImportHue(float degrees)
            => Clamp(Repeat(degrees / HueDegrees + HueNeutral, 1f),
                PostProcessingRules.ColorCurves.CurveMin, PostProcessingRules.ColorCurves.CurveMax);

        /// <summary> That point back to degrees. </summary>
        public static float ExportHue(float hueVsHue)
            => Repeat(hueVsHue - HueNeutral, 1f) * HueDegrees;

        // THE TWO FORMATS MEET AT A FLAT CURVE. Afterbeat's Hue track is one number per keyframe -
        // a rotation applied to every colour equally - and this format's Hue vs Hue is a curve
        // indexed BY the input hue. A single rotation is exactly a flat one, so the conversion is
        // lossless in this direction and lossy only in the other, where a curve that is not flat
        // has no number to become. A rotation of zero converts to NULL rather than to a flat
        // neutral: null is what an untouched control reads as here, and a converted level should
        // not arrive looking edited.

        /// <summary> One Afterbeat hue keyframe as this format's Hue vs Hue curve. </summary>
        public static CurveValue ImportHueCurve(float degrees)
        {
            var value = ImportHue(degrees);
            if (Math.Abs(value - HueNeutral) < float.Epsilon) return null;

            return new CurveValue(new List<CurveKeyframeValue>
            {
                new(ValueRules.MinCurveTime, value),
                new(ValueRules.MaxCurveTime, value),
            }, CurveWrapMode.ClampForever, CurveWrapMode.ClampForever);
        }

        /// <summary> The one rotation a Hue vs Hue curve can be expressed as over there. A curve
        /// that is not flat has no such number, so its FIRST key answers and the rest is lost -
        /// which is what <paramref name="isFlat"/> is for, so a caller can report it. </summary>
        public static float ExportHueCurve(CurveValue curve, out bool isFlat)
        {
            isFlat = true;
            if (curve?.KeyFrames == null || curve.KeyFrames.Count == 0) return ExportHue(HueNeutral);

            var value = curve.KeyFrames[0].Value;
            for (var i = 1; i < curve.KeyFrames.Count; i++)
            {
                if (Math.Abs(curve.KeyFrames[i].Value - value) < float.Epsilon) continue;

                isFlat = false;
                break;
            }
            return ExportHue(value);
        }

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
