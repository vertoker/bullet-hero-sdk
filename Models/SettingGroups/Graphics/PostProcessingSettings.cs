using BHSDK.Models.Enum.Settings;
using Newtonsoft.Json;

namespace BHSDK.Models.SettingGroups.Graphics
{
    public class PostProcessingSettings : BaseGraphicsSettings
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

        public PostProcessingSettings()
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
        public PostProcessingSettings(bool render, bool renderBloom, bool renderChroma, bool renderVignette,
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

        public PostProcessingSettings PresetNone => new()
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
        public PostProcessingSettings PresetMobile => new()
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
        public PostProcessingSettings PresetAll => new()
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
    }
}