using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class Rot : Keyframe, ICopyable<Rot>
    {
        [JsonProperty(Names.Angle)]
        public IFloat Angle { get; set; }

        public Rot()
        {
            Angle = new FloatValue();
        }
        public Rot(int frame, EaseType ease, IFloat angle) 
            : base(frame, ease)
        {
            Angle = angle;
        }

        public Rot Copy() => new(Frame, Ease, Angle.Copy());
    }
}