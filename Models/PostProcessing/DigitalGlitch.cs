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
    public class DigitalGlitch : Keyframe, // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
        ICopyable<DigitalGlitch>, IEquatable<DigitalGlitch>
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

        public override bool Equals(object obj) => obj is DigitalGlitch value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Intensity);

        public bool Equals(DigitalGlitch other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Intensity.Equals(other.Intensity);
            return result;
        }
    }
}