using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    public class MotionBlur : Keyframe // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
    {
        // Quality (client settings variable, he set it himself)
        
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }
        
        // Clamp (0.2f, predefined)

        public MotionBlur()
        {
            Intensity = 1f;
        }
        public MotionBlur(int frame, EaseType ease, float intensity)
            : base(frame, ease)
        {
            Intensity = intensity;
        }
    }
}