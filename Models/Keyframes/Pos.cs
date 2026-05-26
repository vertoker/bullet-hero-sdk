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
    public class Pos : Keyframe, ICopyable<Pos>, IEquatable<Pos>
    {
        [RuleNotNull(typeof(Vector2Value)), RuleIVector2InRange(ValueRules.MinCoord, ValueRules.MaxCoord)]
        [JsonProperty(Names.Vector2)]
        public IVector2 Vector2 { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Anchor)]
        public Alignment Anchor { get; set; }

        public Pos()
        {
            Vector2 = new Vector2Value();
            Anchor = Alignment.CenterMiddle;
        }
        public Pos(IVector2 vector2, Alignment anchor, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Vector2 = vector2;
            Anchor = anchor;
        }

        public object Clone() => Copy();
        public Pos Copy() => new(Vector2.Copy(), Anchor.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is Pos value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Vector2, Anchor);

        public bool Equals(Pos other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Vector2.Equals(other.Vector2)
                         && Anchor.Equals(other.Anchor);
            return result;
        }
    }
}