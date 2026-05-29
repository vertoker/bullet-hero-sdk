using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class Vector2Key : Keyframe, ICopyable<Vector2Key>, IEquatable<Vector2Key>
    {
        [RuleNotNull(typeof(Vector2Value))]
        [JsonProperty(Names.Vector2)]
        public IVector2 Value { get; set; }

        public Vector2Key()
        {
            Value = new Vector2Value();
        }
        public Vector2Key(IVector2 value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Value = value;
        }

        public object Clone() => Copy();
        public Vector2Key Copy() => new(Value.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is Vector2Key value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);

        public bool Equals(Vector2Key other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other) && Value.Equals(other.Value);
            return result;
        }
    }
}