using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class FloatKey : Keyframe, ICopyable<FloatKey>, IEquatable<FloatKey>
    {
        [RuleNotNull(typeof(FloatValue))]
        [JsonProperty(Names.Float)]
        public IFloat Value { get; set; }

        public FloatKey()
        {
            Value = new FloatValue(0f);
        }
        public FloatKey(IFloat value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Value = value;
        }

        public object Clone() => Copy();
        public FloatKey Copy() => new(Value.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is FloatKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);

        public bool Equals(FloatKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Value.Equals(other.Value);
            return result;
        }
    }
}