using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class FloatKey : Keyframe
    {
        [JsonProperty(ModelNames.Float)]
        public IFloat Value { get; set; }

        public FloatKey()
        {
            Value = new FloatValue(0f);
        }
        public FloatKey(int frame, EaseType ease, IFloat value)
            : base(frame, ease)
        {
            Value = value;
        }
    }
}