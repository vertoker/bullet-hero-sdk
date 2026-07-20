using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class FloatMinMax : IFloat, IModel<FloatMinMax>
    {
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.Min)]
        public float Min { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.Max)]
        public float Max { get; set; }

        public FloatMinMax()
        {
            Min = ValueRules.FloatZero;
            Max = ValueRules.FloatOne;
        }
        public FloatMinMax(float min, float max)
        {
            Min = min;
            Max = max;
        }
        public void Reset()
        {
            Min = ValueRules.FloatZero;
            Max = ValueRules.FloatOne;
        }

        public FloatType GetModelType() => FloatType.RandomMinMax;
        
        public object Clone() => Copy();
        IFloat ICopyable<IFloat>.Copy() => new FloatMinMax(Min, Max);
        public FloatMinMax Copy() => new(Min, Max);

        public override bool Equals(object obj) => obj is FloatMinMax value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Min, Max);
        
        public bool Equals(IFloat other) => other is FloatMinMax value && Equals(value);
        public bool Equals(FloatMinMax other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Min.Equals(other.Min)
                         && Max.Equals(other.Max);
            return result;
        }
    }
}