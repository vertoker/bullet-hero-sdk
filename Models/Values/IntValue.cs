using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class IntValue : IInt, ICopyable<IntValue>, IEquatable<IntValue>
    {
        [JsonProperty(Names.ValueShort)]
        public int Value { get; set; }

        public IntValue()
        {
            Value = 0;
        }
        public IntValue(int value)
        {
            Value = value;
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