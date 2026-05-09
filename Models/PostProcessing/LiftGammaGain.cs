using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
using Keyframe = BHSDK.Models.Keyframes.Keyframe;

namespace BHSDK.Models.PostProcessing
{
    public class LiftGammaGain : Keyframe
    {
        [JsonProperty(Names.Lift)]
        public bool Lift { get; set; }
        
        [RuleNotNull] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.LiftColor)]
        public IColor LiftColor { get; set; }
        
        [JsonProperty(Names.Gamma)]
        public bool Gamma { get; set; }
        
        [RuleNotNull] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.GammaColor)]
        public IColor GammaColor { get; set; }
        
        [JsonProperty(Names.Gain)]
        public bool Gain { get; set; }
        
        [RuleNotNull] // TODO add color hdr support for alpha rule (0f-2f)
        [JsonProperty(Names.GainColor)]
        public IColor GainColor { get; set; }

        public LiftGammaGain()
        {
            Lift = false;
            LiftColor = ColorValue.white;
            Gamma = false;
            GammaColor = ColorValue.white;
            Gain = false;
            GainColor = ColorValue.white;
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