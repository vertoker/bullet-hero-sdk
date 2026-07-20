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
    public class AnalogGlitchKey : PostProcessingKeyframe, IModel<AnalogGlitchKey> // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
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

        public AnalogGlitchKey()
        {
            ScanLineJitter = 0.5f;
            VerticalJump = 0f;
            HorizontalShake = 0f;
            ColorDrift = 0f;
        }
        public AnalogGlitchKey(float scanLineJitter, float verticalJump, float horizontalShake, float colorDrift,
            bool active, int frame, EaseType ease = Keyframe.DefaultEase) : base(active, frame, ease)
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
        public override PostProcessingKeyframe Copy() => CopyImpl();
        AnalogGlitchKey ICopyable<AnalogGlitchKey>.Copy() => CopyImpl();
        
        private AnalogGlitchKey CopyImpl() => new(ScanLineJitter, VerticalJump, HorizontalShake, ColorDrift, Active, Frame, Ease);

        public override bool Equals(object obj) => obj is AnalogGlitchKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            ScanLineJitter, VerticalJump, HorizontalShake, ColorDrift);

        public bool Equals(AnalogGlitchKey other)
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