using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
using Keyframe = BHSDK.Models.Keyframes.Keyframe;

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class ShadowsMidtonesHighlights : Keyframe
    {
        [JsonProperty(Names.Shadow)]
        public bool Shadows { get; set; }
        
        [RuleNotNull(typeof(ColorValue))] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.ShadowColor)]
        public IColor ShadowsColor { get; set; }
        
        
        [JsonProperty(Names.Midtone)]
        public bool Midtones { get; set; }
        
        [RuleNotNull(typeof(ColorValue))] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.MidtoneColor)]
        public IColor MidtonesColor { get; set; }
        
        
        [JsonProperty(Names.Highlight)]
        public bool Highlights { get; set; }
        
        [RuleNotNull(typeof(ColorValue))] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.HighlightColor)]
        public IColor HighlightsColor { get; set; }
        
        
        // TODO graph like in Post Processing menu
        
        [RuleNotNull, RuleIVector2InRange(PostProcessingRules.ShadowsMidtonesHighlights.ShadowLimitMin,
             PostProcessingRules.ShadowsMidtonesHighlights.ShadowLimitMax)]
        [JsonProperty(Names.ShadowLimit)]
        public IVector2 ShadowLimits { get; set; }
        
        [RuleNotNull, RuleIVector2InRange(PostProcessingRules.ShadowsMidtonesHighlights.HighlightLimitMin,
             PostProcessingRules.ShadowsMidtonesHighlights.HighlightLimitMax)]
        [JsonProperty(Names.HighlightLimit)]
        public IVector2 HighlightLimits { get; set; }

        public ShadowsMidtonesHighlights()
        {
            Shadows = false;
            ShadowsColor = ColorValue.white;
            Midtones = false;
            MidtonesColor = ColorValue.white;
            Highlights = false;
            HighlightsColor = ColorValue.white;
            
            ShadowLimits = new Vector2Value(0f, 0.3f);
            HighlightLimits = new Vector2Value(0.55f, 1f);
        }
        public ShadowsMidtonesHighlights(int frame, EaseType ease, 
            bool shadows, IColor shadowsColor,
            bool midtones, IColor midtonesColor, 
            bool highlights, IColor highlightsColor, 
            IVector2 shadowLimits, IVector2 highlightLimits)
            : base(frame, ease)
        {
            Shadows = shadows;
            ShadowsColor = shadowsColor;
            Midtones = midtones;
            MidtonesColor = midtonesColor;
            Highlights = highlights;
            HighlightsColor = highlightsColor;
            ShadowLimits = shadowLimits;
            HighlightLimits = highlightLimits;
        }
    }
}