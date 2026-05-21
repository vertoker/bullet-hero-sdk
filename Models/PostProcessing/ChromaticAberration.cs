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
    public class ChromaticAberration : Keyframe,
        ICopyable<ChromaticAberration>, IEquatable<ChromaticAberration>
    {
        [RuleInRange(PostProcessingRules.ChromaticAberration.IntensityMin,
            PostProcessingRules.ChromaticAberration.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }

        public ChromaticAberration()
        {
            Intensity = 1.0f;
        }
        public ChromaticAberration(float intensity,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
        }

        public object Clone() => Copy();
        public ChromaticAberration Copy() => new(Intensity, Frame, Ease);

        public override bool Equals(object obj) => obj is ChromaticAberration value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Intensity);

        public bool Equals(ChromaticAberration other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Intensity.Equals(other.Intensity);
            return result;
        }
    }
}