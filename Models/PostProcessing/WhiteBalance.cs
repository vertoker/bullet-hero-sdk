using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class WhiteBalance : Keyframe
    {
        [RuleInRange(PostProcessingRules.WhiteBalance.TemperatureMin,
            PostProcessingRules.WhiteBalance.TemperatureMax)]
        [JsonProperty(Names.Temperature)]
        public float Temperature { get; set; }
        
        [RuleInRange(PostProcessingRules.WhiteBalance.TintMin,
            PostProcessingRules.WhiteBalance.TintMax)]
        [JsonProperty(Names.Tint)]
        public float Tint { get; set; }

        public WhiteBalance()
        {
            Temperature = 0f;
            Tint = 0f;
        }
        public WhiteBalance(float temperature, float tint,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Temperature = temperature;
            Tint = tint;
        }
    }
}