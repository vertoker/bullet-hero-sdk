using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BHSDK.Models.Base
{
    public abstract class Keyframe : IFrame
    {
        [JsonProperty("f")]
        public int Frame { get; set; }
        
        [JsonProperty("e")]
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