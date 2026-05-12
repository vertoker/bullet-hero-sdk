using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class FilmGrain : Keyframe
    {
        [JsonProperty(Names.Type)]
        public FilmGrainType Type { get; set; }
        
        [RuleInRange(PostProcessingRules.FilmGrain.IntensityMin,
            PostProcessingRules.FilmGrain.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }

        public FilmGrain()
        {
            Type = FilmGrainType.Medium1;
            Intensity = 1.0f;
        }
        public FilmGrain(FilmGrainType type, float intensity,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Type = type;
            Intensity = intensity;
        }
    }
}