using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class AnalogGlitch : Keyframe // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
    {
        [RuleInRange(PostProcessingRules.AnalogGlitch.ScanLineJitterMin,
            PostProcessingRules.AnalogGlitch.ScanLineJitterMax)]
        [JsonProperty(Names.ScanLineJitter)]
        public float ScanLineJitter { get; set; }
        
        [RuleInRange(PostProcessingRules.AnalogGlitch.VerticalJumpMin,
            PostProcessingRules.AnalogGlitch.VerticalJumpMax)]
        [JsonProperty(Names.VerticalJump)]
        public float VerticalJump { get; set; }
        
        [RuleInRange(PostProcessingRules.AnalogGlitch.HorizontalShakeMin,
            PostProcessingRules.AnalogGlitch.HorizontalShakeMax)]
        [JsonProperty(Names.HorizontalShake)]
        public float HorizontalShake { get; set; }
        
        [RuleInRange(PostProcessingRules.AnalogGlitch.ColorDriftMin,
            PostProcessingRules.AnalogGlitch.ColorDriftMax)]
        [JsonProperty(Names.ColorDrift)]
        public float ColorDrift { get; set; }

        public AnalogGlitch()
        {
            ScanLineJitter = 0.5f;
            VerticalJump = 0f;
            HorizontalShake = 0f;
            ColorDrift = 0f;
        }
        public AnalogGlitch(int frame, EaseType ease, float scanLineJitter, 
            float verticalJump, float horizontalShake, float colorDrift) 
            : base(frame, ease)
        {
            ScanLineJitter = scanLineJitter;
            VerticalJump = verticalJump;
            HorizontalShake = horizontalShake;
            ColorDrift = colorDrift;
        }
    }
}