using BHSDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class Bln : IFrame
    {
        [JsonProperty(ModelNames.Frame)]
        public int Frame { get; set; }
        
        [JsonProperty(ModelNames.Boolean)]
        public bool Value { get; set; }

        public Bln()
        {
            Frame = 0;
            Value = false;
        }
        public Bln(int frame, bool value)
        {
            Frame = frame;
            Value = value;
        }
    }
}