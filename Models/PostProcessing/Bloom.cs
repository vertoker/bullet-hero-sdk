using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.PostProcessing
{
    [RuleContainer]
    public class Bloom : Keyframe, // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
        ICopyable<Bloom>, IEquatable<Bloom>
    {
        // Threshold - 0 (always, not a parameter)
        
        [RuleInRange(PostProcessingRules.Bloom.IntensityMin,
           PostProcessingRules.Bloom.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }
        
        [RuleInRange(PostProcessingRules.Bloom.ScatterMin,
            PostProcessingRules.Bloom.ScatterMax)]
        [JsonProperty(Names.Scatter)]
        public float Scatter { get; set; }
        
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.Color)]
        public IColor Color { get; set; }
        
        // Filter (player choose in settings: high - Gaussian, mid - Dual, low - Kawase)

        public Bloom()
        {
            Intensity = 0.5f;
            Scatter = 0.5f;
            Color = ColorValue.red;
        }
        public Bloom(float intensity, float scatter, IColor color,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
            Scatter = scatter;
            Color = color;
        }

        public object Clone() => Copy();
        public Bloom Copy() => new(Intensity, Scatter, Color.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is Bloom value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Intensity, Scatter, Color);

        public bool Equals(Bloom other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Intensity.Equals(other.Intensity)
                         && Scatter.Equals(other.Scatter)
                         && Color.Equals(other.Color);
            return result;
        }
    }
}