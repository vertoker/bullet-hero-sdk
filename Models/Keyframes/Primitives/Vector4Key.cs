using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class Vector4Key : Keyframe, ICopyable<Vector4Key>, IEquatable<Vector4Key>
    {
        [RuleNotNull(typeof(Vector4Value))]
        [JsonProperty(Names.Vector4)]
        public IVector4 Value { get; set; }

        public Vector4Key()
        {
            Value = new Vector4Value();
        }
        public Vector4Key(IVector4 value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Value = value;
        }

        public object Clone() => Copy();
        public Vector4Key Copy() => new(Value.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is Vector4Key value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);

        public bool Equals(Vector4Key other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other) && Value.Equals(other.Value);
            return result;
        }
    }
}