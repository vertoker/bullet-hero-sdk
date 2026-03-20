using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class Rot : Keyframe
    {
        [JsonProperty("a")]
        public IFloat Angle { get; set; } // TODO make IAngle

        public Rot()
        {
            Angle = new FloatValue();
        }
        public Rot(int frame, EaseType ease, IFloat angle) 
            : base(frame, ease)
        {
            Angle = angle;
        }
    }
}