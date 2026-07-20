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
    public class FloatMinMaxStep : IFloat, IModel<FloatMinMaxStep>
    {
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.Min)]
        public float Min { get; set; }
        
        [RuleInRange(ValueRules.MinFloatValue, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.Max)]
        public float Max { get; set; }
        
        [RuleInRange(ValueRules.FloatZero, ValueRules.MaxFloatValue)]
        [JsonProperty(Names.Step)]
        public float Step { get; set; }

        public FloatMinMaxStep()
        {
            Min = ValueRules.FloatZero;
            Max = ValueRules.FloatOne;
            Step = ValueRules.FloatOne;
        }
        public FloatMinMaxStep(float min, float max, float step)
        {
            Min = min;
            Max = max;
            Step = step;
        }
        public void Reset()
        {
            Min = ValueRules.FloatZero;
            Max = ValueRules.FloatOne;
            Step = ValueRules.FloatOne;
        }

        public FloatType GetModelType() => FloatType.RandomMinMaxStep;

        public object Clone() => Copy();
        IFloat ICopyable<IFloat>.Copy() => new FloatMinMaxStep(Min, Max, Step);
        public FloatMinMaxStep Copy() => new(Min, Max, Step);

        public override bool Equals(object obj) => obj is FloatMinMaxStep value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Min, Max, Step);
        
        public bool Equals(IFloat other) => other is FloatMinMaxStep value && Equals(value);
        public bool Equals(FloatMinMaxStep other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Min.Equals(other.Min)
                         && Max.Equals(other.Max)
                         && Step.Equals(other.Step);
            return result;
        }
    }
}