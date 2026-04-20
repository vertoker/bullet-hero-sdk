using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class Flt : Keyframe
    {
        [JsonProperty(ModelNames.Float)]
        public IFloat Value { get; set; }

        public Flt()
        {
            Value = new FloatValue(0f);
        }
        public Flt(int frame, EaseType ease, IFloat value)
            : base(frame, ease)
        {
            Value = value;
        }
    }
}