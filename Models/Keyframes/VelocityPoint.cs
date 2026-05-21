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
    public class VelocityPoint : Keyframe, ICopyable<VelocityPoint>, IEquatable<VelocityPoint>
    {
        [RuleNotNull(typeof(Vector2Value))]
        [JsonProperty(Names.Center)]
        public IVector2 Center { get; set; }
        
        [JsonProperty(Names.Force)]
        public float Force { get; set; }

        public VelocityPoint()
        {
            Center = new Vector2Circle();
            Force = 1;
        }
        public VelocityPoint(IVector2 center, float force, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Center = center;
            Force = force;
        }

        public object Clone() => Copy();
        public VelocityPoint Copy() => new(Center.Copy(), Force, Frame, Ease);

        public override bool Equals(object obj) => obj is VelocityPoint value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Center, Force);

        public bool Equals(VelocityPoint other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Center.Equals(other.Center)
                         && Force.Equals(other.Force);
            return result;
        }
    }
}