using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class Shake : Keyframe
    {
        [JsonProperty("i")]
        public float Intensity { get; set; }
        
        [JsonProperty("s")]
        public float Speed { get; set; }
        
        [JsonProperty("x")]
        public float IntensityX { get; set; }
        
        [JsonProperty("y")]
        public float IntensityY { get; set; }

        public Shake()
        {
            Intensity = 1;
            Speed = 1;
            IntensityX = 1;
            IntensityY = 1;
        }
        public Shake(int frame, EaseType ease, 
            float intensity, float speed, float intensityX, float intensityY) 
            : base(frame, ease)
        {
            Intensity = intensity;
            Speed = speed;
            IntensityX = intensityX;
            IntensityY = intensityY;
        }
    }
}