using System;
using BHSDK.Models.Enum.Settings;
using BHSDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BHSDK.Models.SettingGroups.Graphics
{
    public class PostProcessingGraphicsSettings : BaseGraphicsSettings,
        ICopyable<PostProcessingGraphicsSettings>, IEquatable<PostProcessingGraphicsSettings>
    {
        [JsonProperty(Names.RenderBloom)]
        public bool RenderBloom { get; set; }
        
        [JsonProperty(Names.RenderChroma)]
        public bool RenderChroma { get; set; }
        
        [JsonProperty(Names.RenderVignette)]
        public bool RenderVignette { get; set; }
        
        [JsonProperty(Names.RenderLens)]
        public bool RenderLens { get; set; }
        
        [JsonProperty(Names.RenderGrain)]
        public bool RenderGrain { get; set; }
        
        [JsonProperty(Names.RenderMotionBlur)]
        public bool RenderMotionBlur { get; set; }
        
        [JsonProperty(Names.RenderColorCurves)]
        public bool RenderColorCurves { get; set; }
        
        [JsonProperty(Names.RenderLiftGammaGain)]
        public bool RenderLiftGammaGain { get; set; }
        
        [JsonProperty(Names.RenderShadowsMidtonesHighlights)]
        public bool RenderShadowsMidtonesHighlights { get; set; }
        
        [JsonProperty(Names.RenderWhiteBalance)]
        public bool RenderWhiteBalance { get; set; }
        
        [JsonProperty(Names.RenderAnalogGlitch)]
        public bool RenderAnalogGlitch { get; set; }
        
        [JsonProperty(Names.RenderDigitalGlitch)]
        public bool RenderDigitalGlitch { get; set; }

        public PostProcessingGraphicsSettings()
        {
            RenderBloom = true;
            RenderChroma = true;
            RenderVignette = true;
            RenderLens = true;
            RenderGrain = true;
            RenderMotionBlur = true;
            RenderColorCurves = true;
            RenderLiftGammaGain = true;
            RenderShadowsMidtonesHighlights = true;
            RenderWhiteBalance = true;
            RenderAnalogGlitch = true;
            RenderDigitalGlitch = true;
        }
        public PostProcessingGraphicsSettings(bool render, bool renderBloom, bool renderChroma, bool renderVignette,
            bool renderLens, bool renderGrain, bool renderMotionBlur, bool renderColorCurves, bool renderLiftGammaGain,
            bool renderShadowsMidtonesHighlights, bool renderWhiteBalance, bool renderAnalogGlitch,
            bool renderDigitalGlitch) : base(render)
        {
            RenderBloom = renderBloom;
            RenderChroma = renderChroma;
            RenderVignette = renderVignette;
            RenderLens = renderLens;
            RenderGrain = renderGrain;
            RenderMotionBlur = renderMotionBlur;
            RenderColorCurves = renderColorCurves;
            RenderLiftGammaGain = renderLiftGammaGain;
            RenderShadowsMidtonesHighlights = renderShadowsMidtonesHighlights;
            RenderWhiteBalance = renderWhiteBalance;
            RenderAnalogGlitch = renderAnalogGlitch;
            RenderDigitalGlitch = renderDigitalGlitch;
        }

        public PostProcessingGraphicsSettings PresetNone => new()
        {
            Render = false,
            RenderBloom = false,
            RenderChroma = false,
            RenderVignette = false,
            RenderLens = false,
            RenderGrain = false,
            RenderMotionBlur = false,
            RenderColorCurves = false,
            RenderLiftGammaGain = false,
            RenderShadowsMidtonesHighlights = false,
            RenderWhiteBalance = false,
            RenderAnalogGlitch = false,
            RenderDigitalGlitch = false,
        };
        public PostProcessingGraphicsSettings PresetMobile => new()
        {
            Render = true,
            RenderBloom = false, // HEAVY
            RenderChroma = true,
            RenderVignette = true,
            RenderLens = true,
            RenderGrain = true,
            RenderMotionBlur = false, // HEAVY
            RenderColorCurves = true,
            RenderLiftGammaGain = true,
            RenderShadowsMidtonesHighlights = true,
            RenderWhiteBalance = true,
            RenderAnalogGlitch = false, // HEAVY
            RenderDigitalGlitch = false, // HEAVY
        };
        public PostProcessingGraphicsSettings PresetAll => new()
        {
            Render = true,
            RenderBloom = true,
            RenderChroma = true,
            RenderVignette = true,
            RenderLens = true,
            RenderGrain = true,
            RenderMotionBlur = true,
            RenderColorCurves = true,
            RenderLiftGammaGain = true,
            RenderShadowsMidtonesHighlights = true,
            RenderWhiteBalance = true,
            RenderAnalogGlitch = true,
            RenderDigitalGlitch = true,
        };

        public object Clone() => Copy();
        public PostProcessingGraphicsSettings Copy() => new(Render, RenderBloom, RenderChroma, RenderVignette,
            RenderLens, RenderGrain, RenderMotionBlur, RenderColorCurves, RenderLiftGammaGain,
            RenderShadowsMidtonesHighlights, RenderWhiteBalance, RenderAnalogGlitch, RenderDigitalGlitch);

        public override bool Equals(object obj) => obj is PostProcessingGraphicsSettings value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(RenderBloom);
            hashCode.Add(RenderChroma);
            hashCode.Add(RenderVignette);
            hashCode.Add(RenderLens);
            hashCode.Add(RenderGrain);
            hashCode.Add(RenderMotionBlur);
            hashCode.Add(RenderColorCurves);
            hashCode.Add(RenderLiftGammaGain);
            hashCode.Add(RenderShadowsMidtonesHighlights);
            hashCode.Add(RenderWhiteBalance);
            hashCode.Add(RenderAnalogGlitch);
            hashCode.Add(RenderDigitalGlitch);
            return hashCode.ToHashCode();
        }

        public bool Equals(PostProcessingGraphicsSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other)
                   && RenderBloom == other.RenderBloom
                   && RenderChroma == other.RenderChroma
                   && RenderVignette == other.RenderVignette
                   && RenderLens == other.RenderLens
                   && RenderGrain == other.RenderGrain
                   && RenderMotionBlur == other.RenderMotionBlur
                   && RenderColorCurves == other.RenderColorCurves
                   && RenderLiftGammaGain == other.RenderLiftGammaGain
                   && RenderShadowsMidtonesHighlights == other.RenderShadowsMidtonesHighlights
                   && RenderWhiteBalance == other.RenderWhiteBalance
                   && RenderAnalogGlitch == other.RenderAnalogGlitch
                   && RenderDigitalGlitch == other.RenderDigitalGlitch;
        }
    }
}