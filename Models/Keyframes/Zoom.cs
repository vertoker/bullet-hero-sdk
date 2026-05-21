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
    public class Zoom : Keyframe, ICopyable<Zoom>, IEquatable<Zoom>
    {
        [RuleNotNull(typeof(FloatValue)), RuleIFloatInRange(ValueRules.MinZoom, ValueRules.MaxZoom)]
        [JsonProperty(Names.Size)]
        public IFloat Size { get; set; }

        public Zoom()
        {
            Size = new FloatValue();
        }
        public Zoom(IFloat size, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Size = size;
        }

        public object Clone() => Copy();
        public Zoom Copy() => new(Size.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is Zoom value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Size);

        public bool Equals(Zoom other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Size.Equals(other.Size);
            return result;
        }
    }
}