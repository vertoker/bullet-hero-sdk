using System;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Keyframes
{
    [RuleContainer]
    public abstract class Keyframe : IFrame, IEquatable<Keyframe>
    {
        public const EaseType DefaultEase = EaseType.Linear;
        
        [RuleLevelFrame]
        [JsonProperty(Names.FrameShort)]
        public int Frame { get; set; }
        
        [JsonProperty(Names.Ease)]
        public EaseType Ease { get; set; }

        protected Keyframe()
        {
            Frame = 0;
            Ease = EaseType.Linear;
        }
        protected Keyframe(int frame, EaseType ease = DefaultEase)
        {
            Frame = frame;
            Ease = ease;
        }

        public override bool Equals(object obj) => obj is Keyframe value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Frame, (int)Ease);

        public bool Equals(Keyframe other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Frame.Equals(other.Frame)
                         && Ease == other.Ease;
            return result;
        }
    }
}