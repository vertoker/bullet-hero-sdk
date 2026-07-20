using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.PostProcessing
{
    public class PostProcessingKeyframe : IKeyframe, IModel<PostProcessingKeyframe>
    {
        [JsonProperty(Names.ActiveShort)]
        public bool Active { get; set; }
        
        [RuleLevelFrame]
        [JsonProperty(Names.FrameShort)]
        public int Frame { get; set; }
        
        [JsonProperty(Names.Ease)]
        public EaseType Ease { get; set; }

        public PostProcessingKeyframe()
        {
            Active = PostProcessingRules.ActiveDefault;
            Frame = Keyframe.DefaultFrame;
            Ease = Keyframe.DefaultEase;
        }
        public PostProcessingKeyframe(bool active, int frame, EaseType ease = Keyframe.DefaultEase)
        {
            Active = active;
            Frame = frame;
            Ease = ease;
        }
        public virtual void Reset()
        {
            Frame = Keyframe.DefaultFrame;
            Ease = Keyframe.DefaultEase;
            Active = PostProcessingRules.ActiveDefault;
        }

        public virtual object Clone() => CopyImpl();
        public virtual PostProcessingKeyframe Copy() => CopyImpl();
        
        private PostProcessingKeyframe CopyImpl() => new(Active, Frame, Ease);

        public override bool Equals(object obj) => obj is PostProcessingKeyframe value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Active, Frame, (int)Ease);

        public bool Equals(PostProcessingKeyframe other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Active == other.Active
                         && Frame.Equals(other.Frame)
                         && Ease == other.Ease;
            return result;
        }
    }
}