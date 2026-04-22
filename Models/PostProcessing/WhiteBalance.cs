using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    public class WhiteBalance : Keyframe
    {
        [JsonProperty(Names.Temperature)]
        public float Temperature { get; set; }
        
        [JsonProperty(Names.Tint)]
        public float Tint { get; set; }

        public WhiteBalance()
        {
            Temperature = 0f;
            Tint = 0f;
        }
        public WhiteBalance(int frame, EaseType ease, 
            float temperature, float tint)
            : base(frame, ease)
        {
            Temperature = temperature;
            Tint = tint;
        }
    }
}