using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    public class DigitalGlitch : Keyframe // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
    {
        [RuleInRange(PostProcessingRules.DigitalGlitch.IntensityMin,
            PostProcessingRules.DigitalGlitch.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }

        public DigitalGlitch()
        {
            Intensity = 0.1f;
        }
        public DigitalGlitch(int frame, EaseType ease, float intensity)
            : base(frame, ease)
        {
            Intensity = intensity;
        }
    }
}