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
        [JsonProperty(ModelNames.Lift)]
        public bool Lift { get; set; }
        
        [JsonProperty(ModelNames.Lift + ModelNames.Color)]
        public IColor LiftColor { get; set; }
        
        
        [JsonProperty(ModelNames.Gamma)]
        public bool Gamma { get; set; }
        
        [JsonProperty(ModelNames.Gamma + ModelNames.Color)]
        public IColor GammaColor { get; set; }
        
        
        [JsonProperty(ModelNames.Gain)]
        public bool Gain { get; set; }
        
        [JsonProperty(ModelNames.Gain + ModelNames.Color)]
        public IColor GainColor { get; set; }

        public LiftGammaGain()
        {
            Lift = false;
            LiftColor = new ColorValue(Color.white);
            Gamma = false;
            GammaColor = new ColorValue(Color.white);
            Gain = false;
            GainColor = new ColorValue(Color.white);
        }
        public LiftGammaGain(int frame, EaseType ease, 
            bool lift, IColor liftColor,
            bool gamma, IColor gammaColor,
            bool gain, IColor gainColor)
            : base(frame, ease)
        {
            Lift = lift;
            LiftColor = liftColor;
            Gamma = gamma;
            GammaColor = gammaColor;
            Gain = gain;
            GainColor = gainColor;
        }
    }
}