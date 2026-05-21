using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class FloatValue : IFloat, ICopyable<FloatValue>, IEquatable<FloatValue>
    {
        [JsonProperty(Names.ValueShort)]
        public float Value { get; set; }

        public FloatValue()
        {
            Value = 0f;
        }
        public FloatValue(float value)
        {
            Value = value;
        }

        public FloatType GetModelType() => FloatType.Value;
        
        public object Clone() => Copy();
        IFloat ICopyable<IFloat>.Copy() => new FloatValue(Value);
        public FloatValue Copy() => new(Value);
        
        public bool Equals(IFloat other) => other is FloatValue value && Equals(value);
        public bool Equals(FloatValue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Value.Equals(other.Value);
            return result;
        }

        public override bool Equals(object obj) => obj is FloatValue value && Equals(value);
        public override int GetHashCode() => Value.GetHashCode();
    }
}