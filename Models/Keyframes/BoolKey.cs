using BHSDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class BoolKey : IFrame
    {
        [JsonProperty(ModelNames.Frame)]
        public int Frame { get; set; }
        
        [JsonProperty(ModelNames.Boolean)]
        public bool Value { get; set; }

        public BoolKey()
        {
            Frame = 0;
            Value = false;
        }
        public BoolKey(int frame, bool value)
        {
            Frame = frame;
            Value = value;
        }
    }
}