using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;
using Keyframe = BHSDK.Models.Keyframes.Keyframe;
using Keyframes_Keyframe = BHSDK.Models.Keyframes.Keyframe;

namespace BHSDK.Models.PostProcessing
{
    public class LiftGammaGain : Keyframes_Keyframe
    {
        [JsonProperty(Names.Lift)]
        public bool Lift { get; set; }
        
        [JsonProperty(Names.LiftColor)]
        public IColor LiftColor { get; set; }
        
        [JsonProperty(Names.Gamma)]
        public bool Gamma { get; set; }
        
        [JsonProperty(Names.GammaColor)]
        public IColor GammaColor { get; set; }
        
        [JsonProperty(Names.Gain)]
        public bool Gain { get; set; }
        
        [JsonProperty(Names.GainColor)]
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