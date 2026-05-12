using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class MotionBlur : Keyframe, ICopyable<MotionBlur> // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
    {
        // Quality (client settings variable, he set it himself)
        
        [RuleInRange(PostProcessingRules.MotionBlur.IntensityMin,
            PostProcessingRules.MotionBlur.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }
        
        // Clamp (0.2f, predefined)

        public MotionBlur()
        {
            Intensity = 1f;
        }
        public MotionBlur(float intensity,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
        }

        public object Clone() => Copy();
        public MotionBlur Copy() => new(Intensity, Frame, Ease);
    }
}