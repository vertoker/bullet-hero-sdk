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
    public class IntKey : Keyframe, ICopyable<IntKey>, IEquatable<IntKey>
    {
        [RuleNotNull(typeof(IntValue))]
        [JsonProperty(Names.Int)]
        public IInt Value { get; set; }

        public IntKey()
        {
            Value = new IntValue(0);
        }
        public IntKey(IInt value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Value = value;
        }

        public object Clone() => Copy();
        public IntKey Copy() => new(Value.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is IntKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value);

        public bool Equals(IntKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Value.Equals(other.Value);
            return result;
        }
    }
}