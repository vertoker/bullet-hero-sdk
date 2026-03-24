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
        [JsonProperty(ModelNames.Shadow)]
        public bool Shadows { get; set; }
        
        [JsonProperty(ModelNames.Shadow + ModelNames.Color)]
        public IColor ShadowsColor { get; set; }
        
        
        [JsonProperty(ModelNames.Midtone)]
        public bool Midtones { get; set; }
        
        [JsonProperty(ModelNames.Midtone + ModelNames.Color)]
        public IColor MidtonesColor { get; set; }
        
        
        [JsonProperty(ModelNames.Highlight)]
        public bool Highlights { get; set; }
        
        [JsonProperty(ModelNames.Highlight + ModelNames.Color)]
        public IColor HighlightsColor { get; set; }
        
        
        // TODO graph like in Post Processing menu
        
        [JsonProperty(ModelNames.Shadow + ModelNames.Limit)]
        public IVector2 ShadowLimits { get; set; }
        
        [JsonProperty(ModelNames.Highlight + ModelNames.Limit)]
        public IVector2 HighlightLimits { get; set; }

        public ShadowsMidtonesHighlights()
        {
            Shadows = false;
            ShadowsColor = new ColorValue(Color.white);
            Midtones = false;
            MidtonesColor = new ColorValue(Color.white);
            Highlights = false;
            HighlightsColor = new ColorValue(Color.white);
            
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