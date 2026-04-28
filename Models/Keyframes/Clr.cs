using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class Clr : Keyframe, ICopyable<Clr>
    {
        [JsonProperty(Names.Color)]
        public IColor Value { get; set; }

        public Clr()
        {
            Value = new ColorValue(UnityEngine.Color.white);
        }
        public Clr(int frame, EaseType ease, IColor value)
            : base(frame, ease)
        {
            Value = value;
        }

        public Clr Copy() => new(Frame, Ease, Value.Copy());
    }
}