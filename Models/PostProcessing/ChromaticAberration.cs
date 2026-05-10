using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class ChromaticAberration : Keyframe
    {
        [RuleInRange(PostProcessingRules.ChromaticAberration.IntensityMin,
            PostProcessingRules.ChromaticAberration.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }

        public ChromaticAberration()
        {
            Intensity = 1.0f;
        }
        public ChromaticAberration(int frame, 
            EaseType ease, float intensity)
            : base(frame, ease)
        {
            Intensity = intensity;
        }
    }
}