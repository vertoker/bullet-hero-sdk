using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class GradientColorKeyValue : IModel<GradientColorKeyValue>
    {
        // TODO maybe replace FloatValue to IFloat (color too) (in editor step)
        
        [RuleNotNull]
        [JsonProperty(Names.Color)]
        public ColorValue Color { get; set; }
        
        [RuleInRange(ValueRules.MinGradientTime, ValueRules.MaxGradientTime)]
        [JsonProperty(Names.TimeShort)]
        public float Time { get; set; }
        
        public GradientColorKeyValue()
        {
            Color = ColorValue.white;
            Time = ValueRules.FloatZero;
        }
        public GradientColorKeyValue(ColorValue color, float time)
        {
            Color = color;
            Time = time;
        }
        public void Reset()
        {
            Color = ColorValue.white;
            Time = ValueRules.FloatZero;
        }

        public object Clone() => Copy();
        public GradientColorKeyValue Copy() => new(Color.Copy(), Time);

        public override bool Equals(object obj) => obj is GradientColorKeyValue value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Color, Time);

        public bool Equals(GradientColorKeyValue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Color.Equals(other.Color)
                         && Time.Equals(other.Time);
            return result;
        }
    }
}