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
    public class IntMinMaxStep : IInt, IModel<IntMinMaxStep>
    {
        [RuleInRange(ValueRules.MinIntValue, ValueRules.MaxIntValue)]
        [JsonProperty(Names.Min)]
        public int Min { get; set; }
        
        [RuleInRange(ValueRules.MinIntValue, ValueRules.MaxIntValue)]
        [JsonProperty(Names.Max)]
        public int Max { get; set; }
        
        [RuleInRange(ValueRules.IntZero, ValueRules.MaxIntValue)]
        [JsonProperty(Names.Step)]
        public int Step { get; set; }

        public IntMinMaxStep()
        {
            Min = ValueRules.IntZero;
            Max = ValueRules.IntOne;
            Step = ValueRules.IntOne;
        }
        public IntMinMaxStep(int min, int max, int step)
        {
            Min = min;
            Max = max;
            Step = step;
        }
        public void Reset()
        {
            Min = ValueRules.IntZero;
            Max = ValueRules.IntOne;
            Step = ValueRules.IntOne;
        }

        public IntType GetModelType() => IntType.RandomMinMaxStep;

        public object Clone() => Copy();
        IInt ICopyable<IInt>.Copy() => new IntMinMaxStep(Min, Max, Step);
        public IntMinMaxStep Copy() => new(Min, Max, Step);

        public override bool Equals(object obj) => obj is IntMinMaxStep value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Min, Max, Step);
        
        public bool Equals(IInt other) => other is IntMinMaxStep value && Equals(value);
        public bool Equals(IntMinMaxStep other)
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