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
    public class StringValue : IString, ICopyable<StringValue>, IEquatable<StringValue>
    {
        [RuleNotNull, RuleStringMax(ValueRules.MaxGameString)]
        [JsonProperty(Names.ValueShort)]
        public string Value { get; set; }

        public StringValue()
        {
            Value = string.Empty;
        }
        public StringValue(string value)
        {
            Value = value;
        }

        public StringType GetModelType() => StringType.Value;
        
        public object Clone() => Copy();
        IString ICopyable<IString>.Copy() => new StringValue(Value);
        public StringValue Copy() => new(Value);

        public override bool Equals(object obj) => obj is StringValue value && Equals(value);
        public override int GetHashCode() => Value != null ? Value.GetHashCode() : 0;
        
        public bool Equals(IString other) => other is StringValue value && Equals(value);
        public bool Equals(StringValue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Value.Equals(other.Value);
            return result;
        }
    }
}