using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public abstract class Keyframe : IFrame
    {
        [JsonProperty(ModelNames.Frame)]
        public int Frame { get; set; }
        
        [JsonProperty(ModelNames.Ease)]
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