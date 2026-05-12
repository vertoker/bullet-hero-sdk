using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class DigitalGlitch : Keyframe, ICopyable<DigitalGlitch> // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
    {
        [RuleInRange(PostProcessingRules.DigitalGlitch.IntensityMin,
            PostProcessingRules.DigitalGlitch.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }

        public DigitalGlitch()
        {
            Intensity = 0.1f;
        }
        public DigitalGlitch(float intensity,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
        }

        public object Clone() => Copy();
        public DigitalGlitch Copy() => new(Intensity, Frame, Ease);
    }
}