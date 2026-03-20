using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;
using Keyframe = BHSDK.Models.Base.Keyframe;

namespace BHSDK.Models.PostProcessing
{
    public class LiftGammaGain : Keyframe
    {
        [JsonProperty("l")]
        public bool Lift { get; set; }
        
        [JsonProperty("lc")]
        public IColor LiftColor { get; set; } // TODO only RGB
        
        [JsonProperty("la")]
        public float LiftAlpha { get; set; }
        
        
        [JsonProperty("gm")]
        public bool Gamma { get; set; }
        
        [JsonProperty("gmc")]
        public IColor GammaColor { get; set; } // TODO only RGB
        
        [JsonProperty("gma")]
        public float GammaAlpha { get; set; }
        
        
        [JsonProperty("gn")]
        public bool Gain { get; set; }
        
        [JsonProperty("gnc")]
        public IColor GainColor { get; set; } // TODO only RGB
        
        [JsonProperty("gna")]
        public float GainAlpha { get; set; }

        public LiftGammaGain()
        {
            Lift = false;
            LiftColor = new ColorValue(Color.white);
            LiftAlpha = 1f;
            Gamma = false;
            GammaColor = new ColorValue(Color.white);
            GammaAlpha = 1f;
            Gain = false;
            GainColor = new ColorValue(Color.white);
            GainAlpha = 1f;
        }
        public LiftGammaGain(int frame, EaseType ease, 
            bool lift, IColor liftColor, float liftAlpha, 
            bool gamma, IColor gammaColor, float gammaAlpha, 
            bool gain, IColor gainColor, float gainAlpha)
            : base(frame, ease)
        {
            Lift = lift;
            LiftColor = liftColor;
            LiftAlpha = liftAlpha;
            Gamma = gamma;
            GammaColor = gammaColor;
            GammaAlpha = gammaAlpha;
            Gain = gain;
            GainColor = gainColor;
            GainAlpha = gainAlpha;
        }
    }
}