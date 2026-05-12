using System;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Keyframes
{
    [RuleContainer]
    public class Sca : Keyframe, ICopyable<Sca>, IEquatable<Sca>
    {
        [RuleNotNull(typeof(Vector2Value)), RuleIVector2InRange(ValueRules.MinSca, ValueRules.MaxSca)]
        [JsonProperty(Names.Vector2)]
        public IVector2 Vector2 { get; set; }

        public Sca()
        {
            Vector2 = new Vector2Circle();
        }
        public Sca(IVector2 vector2, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Vector2 = vector2;
        }

        public object Clone() => Copy();
        public Sca Copy() => new(Vector2.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is Sca value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Vector2);

        public bool Equals(Sca other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Vector2.Equals(other.Vector2);
            return result;
        }
    }
}