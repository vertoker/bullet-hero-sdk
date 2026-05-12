using System;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class AnalogGlitch : Keyframe, // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
        ICopyable<AnalogGlitch>, IEquatable<AnalogGlitch>
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
        public AnalogGlitch(float scanLineJitter, float verticalJump, float horizontalShake, float colorDrift,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            ScanLineJitter = scanLineJitter;
            VerticalJump = verticalJump;
            HorizontalShake = horizontalShake;
            ColorDrift = colorDrift;
        }

        public object Clone() => Copy();
        public AnalogGlitch Copy() => new(ScanLineJitter, VerticalJump, HorizontalShake, ColorDrift, Frame, Ease);

        public override bool Equals(object obj) => obj is AnalogGlitch value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            ScanLineJitter, VerticalJump, HorizontalShake, ColorDrift);

        public bool Equals(AnalogGlitch other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && ScanLineJitter.Equals(other.ScanLineJitter)
                         && VerticalJump.Equals(other.VerticalJump)
                         && HorizontalShake.Equals(other.HorizontalShake)
                         && ColorDrift.Equals(other.ColorDrift);
            return result;
        }
    }
}