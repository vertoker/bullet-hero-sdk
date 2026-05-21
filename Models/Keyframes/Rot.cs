using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class Rot : Keyframe, ICopyable<Rot>, IEquatable<Rot>
    {
        [RuleNotNull(typeof(FloatValue)), RuleIFloatInRange(ValueRules.MinCoord, ValueRules.MaxCoord)]
        [JsonProperty(Names.Angle)]
        public IFloat Angle { get; set; }

        public Rot()
        {
            Angle = new FloatValue();
        }
        public Rot(IFloat angle, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Angle = angle;
        }

        public object Clone() => Copy();
        public Rot Copy() => new(Angle.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is Rot value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Angle);

        public bool Equals(Rot other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Angle.Equals(other.Angle);
            return result;
        }
    }
}