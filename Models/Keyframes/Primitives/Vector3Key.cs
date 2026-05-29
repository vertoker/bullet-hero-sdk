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
    public class Vector3Key : Keyframe, ICopyable<Vector3Key>, IEquatable<Vector3Key>
    {
        [RuleNotNull(typeof(Vector3Value))]
        [JsonProperty(Names.Vector3)]
        public IVector3 Value { get; set; }

        public Vector3Key()
        {
            Value = new Vector3Value();
        }
        public Vector3Key(IVector3 value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Value = value;
        }

        public object Clone() => Copy();
        public Vector3Key Copy() => new(Value.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is Vector3Key value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);

        public bool Equals(Vector3Key other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other) && Value.Equals(other.Value);
            return result;
        }
    }
}