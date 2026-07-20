using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class Keyframe : IKeyframe, IModel<Keyframe>
    {
        public const int DefaultFrame = 0;
        public const EaseType DefaultEase = EaseType.Linear;
        
        [RuleLevelFrame]
        [JsonProperty(Names.FrameShort)]
        public int Frame { get; set; }
        
        [JsonProperty(Names.Ease)]
        public EaseType Ease { get; set; }

        protected Keyframe()
        {
            Frame = DefaultFrame;
            Ease = DefaultEase;
        }
        protected Keyframe(int frame, EaseType ease = DefaultEase)
        {
            Frame = frame;
            Ease = ease;
        }
        public virtual void Reset()
        {
            Frame = DefaultFrame;
            Ease = DefaultEase;
        }

        public virtual object Clone() => CopyImpl();
        public virtual Keyframe Copy() => CopyImpl();
        
        private Keyframe CopyImpl() => new(Frame, Ease);

        public override bool Equals(object obj) => obj is Keyframe value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Frame, (int)Ease);

        public bool Equals(Keyframe other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Frame.Equals(other.Frame) && Ease == other.Ease;
            return result;
        }
    }
}