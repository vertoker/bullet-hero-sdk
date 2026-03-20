using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    public class AnalogGlitch : Keyframe // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
    {
        [JsonProperty("slj")]
        public float ScanLineJitter { get; set; }
        
        [JsonProperty("vj")]
        public float VerticalJump { get; set; }
        
        [JsonProperty("hs")]
        public float HorizontalShake { get; set; }
        
        [JsonProperty("cd")]
        public float ColorDrift { get; set; }

        public AnalogGlitch()
        {
            ScanLineJitter = 0.5f;
            VerticalJump = 0f;
            HorizontalShake = 0f;
            ColorDrift = 0f;
        }
        public AnalogGlitch(int frame, EaseType ease, float scanLineJitter, 
            float verticalJump, float horizontalShake, float colorDrift) 
            : base(frame, ease)
        {
            ScanLineJitter = scanLineJitter;
            VerticalJump = verticalJump;
            HorizontalShake = horizontalShake;
            ColorDrift = colorDrift;
        }
    }
}