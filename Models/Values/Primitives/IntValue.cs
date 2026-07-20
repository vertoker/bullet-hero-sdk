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
    public class IntValue : IInt, IModel<IntValue>
    {
        [RuleInRange(ValueRules.MinIntValue, ValueRules.MaxIntValue)]
        [JsonProperty(Names.ValueShort)]
        public int Value { get; set; }

        public IntValue()
        {
            Value = ValueRules.IntZero;
        }
        public IntValue(int value)
        {
            Value = value;
        }
        public void Reset()
        {
            Value = ValueRules.IntZero;
        }

        public IntType GetModelType() => IntType.Value;
        
        public object Clone() => Copy();
        IInt ICopyable<IInt>.Copy() => new IntValue(Value);
        public IntValue Copy() => new(Value);

        public override bool Equals(object obj) => obj is IntValue value && Equals(value);
        public override int GetHashCode() => Value;

        public bool Equals(IInt other) => other is IntValue value && Equals(value);
        public bool Equals(IntValue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Value.Equals(other.Value);
            return result;
        }
    }
}