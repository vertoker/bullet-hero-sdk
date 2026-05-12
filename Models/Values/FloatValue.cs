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