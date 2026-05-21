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
    // TODO activate for player when add events
    [RuleContainer]
    public class Velocity : Keyframe, ICopyable<Velocity>, IEquatable<Velocity>
    {
        [RuleNotNull(typeof(Vector2Value))]
        [JsonProperty(Names.Vector2)]
        public IVector2 Vector2 { get; set; }

        public Velocity()
        {
            Vector2 = new Vector2Circle();
        }
        public Velocity(IVector2 vector2, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Vector2 = vector2;
        }

        public object Clone() => Copy();
        public Velocity Copy() => new(Vector2.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is Velocity value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Vector2);

        public bool Equals(Velocity other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Vector2.Equals(other.Vector2);
            return result;
        }
    }
}