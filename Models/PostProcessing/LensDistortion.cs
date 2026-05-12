using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class LensDistortion : Keyframe, ICopyable<LensDistortion>
    {
        [RuleInRange(PostProcessingRules.LensDistortion.IntensityMin,
            PostProcessingRules.LensDistortion.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }
        
        [RuleNotNull(typeof(Vector2Value)), RuleIVector2InRange(PostProcessingRules.LensDistortion.MultiplierMin,
             PostProcessingRules.LensDistortion.MultiplierMax)]
        [JsonProperty(Names.Multiplier)]
        public IVector2 Multiplier { get; set; }
        
        [RuleNotNull(typeof(Vector2Value)), RuleIVector2InRange(PostProcessingRules.LensDistortion.CenterMin,
             PostProcessingRules.LensDistortion.CenterMax)]
        [JsonProperty(Names.Center)]
        public IVector2 Center { get; set; }
        
        [RuleInRange(PostProcessingRules.LensDistortion.ScaleMin,
            PostProcessingRules.LensDistortion.ScaleMax)]
        [JsonProperty(Names.Scale)]
        public float Scale { get; set; }

        public LensDistortion()
        {
            Intensity = 0.5f;
            Multiplier = new Vector2Value(1f, 1f);
            Center = new Vector2Value(0.5f, 0.5f);
            Scale = 1f;
        }
        public LensDistortion(float intensity, IVector2 multiplier, IVector2 center, float scale,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
            Multiplier = multiplier;
            Center = center;
            Scale = scale;
        }

        public object Clone() => Copy();
        public LensDistortion Copy() => new(Intensity, Multiplier.Copy(), Center.Copy(), Scale, Frame, Ease);
    }
}