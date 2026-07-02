using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class UVKey : Keyframe, ICopyable<UVKey>, IEquatable<UVKey>
    {
        [RuleNotNull]
        [JsonProperty(Names.Tilling)]
        public Vector2Value Tilling { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Offset)]
        public Vector2Value Offset { get; set; }

        public UVKey()
        {
            Tilling = new Vector2Value(ValueRules.DefaultUvX, ValueRules.DefaultUvY);
            Offset = new Vector2Value(ValueRules.DefaultUvZ, ValueRules.DefaultUvW);
        }
        public UVKey(Vector2Value tilling, Vector2Value offset, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Tilling = tilling;
            Offset = offset;
        }

        public object Clone() => Copy();
        public UVKey Copy() => new(Tilling.Copy(), Offset.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is UVKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Tilling, Offset);

        public bool Equals(UVKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Tilling.Equals(other.Tilling)
                         && Offset.Equals(other.Offset);
            return result;
        }
    }
}