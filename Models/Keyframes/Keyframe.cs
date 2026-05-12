using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    [RuleContainer]
    public abstract class Keyframe : IFrame
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
    }
}