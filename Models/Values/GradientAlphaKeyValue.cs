using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class GradientAlphaKeyValue : ICopyable<GradientAlphaKeyValue>, IEquatable<GradientAlphaKeyValue>
    {
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.AlphaShort)]
        public float Alpha { get; set; }
        
        [RuleInRange(ValueRules.MinGradientTime, ValueRules.MaxGradientTime)]
        [JsonProperty(Names.TimeShort)]
        public float Time { get; set; }
        
        public GradientAlphaKeyValue()
        {
            Alpha = 1f;
            Time = 0f;
        }
        public GradientAlphaKeyValue(float alpha, float time)
        {
            Alpha = alpha;
            Time = time;
        }

        public object Clone() => Copy();
        public GradientAlphaKeyValue Copy() => new(Alpha, Time);

        public override bool Equals(object obj) => obj is GradientAlphaKeyValue value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Alpha, Time);

        public bool Equals(GradientAlphaKeyValue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Alpha.Equals(other.Alpha)
                         && Time.Equals(other.Time);
            return result;
        }
    }
}