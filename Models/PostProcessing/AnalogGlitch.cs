using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.PostProcessing
{
    [RuleContainer]
    public class AnalogGlitch : Keyframe, IModel<AnalogGlitch> // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
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
        public override void Reset()
        {
            base.Reset();
            ScanLineJitter = 0.5f;
            VerticalJump = 0f;
            HorizontalShake = 0f;
            ColorDrift = 0f;
        }
        
        public override object Clone() => CopyImpl();
        public override Keyframe Copy() => CopyImpl();
        AnalogGlitch ICopyable<AnalogGlitch>.Copy() => CopyImpl();
        
        private AnalogGlitch CopyImpl() => new(ScanLineJitter, VerticalJump, HorizontalShake, ColorDrift, Frame, Ease);

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