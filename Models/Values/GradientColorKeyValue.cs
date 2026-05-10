using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class GradientColorKeyValue : ICopyable<GradientColorKeyValue>
    {
        // TODO maybe replace FloatValue to IFloat (color too) (in editor step)
        
        [RuleNotNull]
        [JsonProperty(Names.Color)]
        public ColorValue ColorHDR { get; set; }
        
        [JsonProperty(Names.TimeShort)]
        public float Time { get; set; }
        
        public GradientColorKeyValue()
        {
            ColorHDR = ColorValue.white;
            Time = 0f;
        }
        public GradientColorKeyValue(ColorValue colorHDR, float time)
        {
            ColorHDR = colorHDR;
            Time = time;
        }

        public GradientColorKeyValue Copy() => new(ColorHDR.Copy(), Time);
    }
}