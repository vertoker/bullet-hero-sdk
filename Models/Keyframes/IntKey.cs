using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class IntKey : Keyframe
    {
        [JsonProperty(ModelNames.Float)]
        public IInt Value { get; set; }

        public IntKey()
        {
            Value = new IntValue(0);
        }
        public IntKey(int frame, EaseType ease, IInt value)
            : base(frame, ease)
        {
            Value = value;
        }
    }
}