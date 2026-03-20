using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;
using Keyframe = BHSDK.Models.Base.Keyframe;

namespace BHSDK.Models.PostProcessing
{
    public class ShadowsMidtonesHighlights : Keyframe
    {
        [JsonProperty("s")]
        public bool Shadows { get; set; }
        
        [JsonProperty("sc")]
        public IColor ShadowsColor { get; set; } // TODO only RGB
        
        [JsonProperty("sa")]
        public float ShadowsAlpha { get; set; }
        
        
        [JsonProperty("m")]
        public bool Midtones { get; set; }
        
        [JsonProperty("mc")]
        public IColor MidtonesColor { get; set; } // TODO only RGB
        
        [JsonProperty("ma")]
        public float MidtonesAlpha { get; set; }
        
        
        [JsonProperty("h")]
        public bool Highlights { get; set; }
        
        [JsonProperty("hc")]
        public IColor HighlightsColor { get; set; } // TODO only RGB
        
        [JsonProperty("ha")]
        public float HighlightsAlpha { get; set; }
        
        // TODO graph like in Post Processing menu
        
        [JsonProperty("sl")]
        public IVector ShadowLimits { get; set; }
        
        [JsonProperty("hl")]
        public IVector HighlightLimits { get; set; }

        public ShadowsMidtonesHighlights()
        {
            Shadows = false;
            ShadowsColor = new ColorValue(Color.white);
            ShadowsAlpha = 1f;
            
            Midtones = false;
            MidtonesColor = new ColorValue(Color.white);
            MidtonesAlpha = 1f;
            
            Highlights = false;
            HighlightsColor = new ColorValue(Color.white);
            HighlightsAlpha = 1f;

            ShadowLimits = new VectorValue(0f, 0.3f);
            HighlightLimits = new VectorValue(0.55f, 1f);
        }
        public ShadowsMidtonesHighlights(int frame, EaseType ease, 
            bool shadows, IColor shadowsColor, float shadowsAlpha, 
            bool midtones, IColor midtonesColor, float midtonesAlpha, 
            bool highlights, IColor highlightsColor, float highlightsAlpha, 
            IVector shadowLimits, IVector highlightLimits)
            : base(frame, ease)
        {
            Shadows = shadows;
            ShadowsColor = shadowsColor;
            ShadowsAlpha = shadowsAlpha;
            Midtones = midtones;
            MidtonesColor = midtonesColor;
            MidtonesAlpha = midtonesAlpha;
            Highlights = highlights;
            HighlightsColor = highlightsColor;
            HighlightsAlpha = highlightsAlpha;
            ShadowLimits = shadowLimits;
            HighlightLimits = highlightLimits;
        }
    }
}