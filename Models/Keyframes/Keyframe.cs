using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public abstract class Keyframe : IFrame
    {
        // TODO make all frames to local frames
        [JsonProperty(Names.FrameShort)]
        public int Frame { get; set; }
        
        [JsonProperty(Names.Ease)]
        public EaseType Ease { get; set; }

        protected Keyframe()
        {
            Frame = 0;
            Ease = EaseType.Linear;
        }
        protected Keyframe(int frame, EaseType ease)
        {
            Frame = frame;
            Ease = ease;
        }
    }
}