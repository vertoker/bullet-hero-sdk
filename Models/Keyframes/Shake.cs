using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class Shake : Keyframe
    {
        [JsonProperty(ModelNames.Intensity)]
        public float Intensity { get; set; }
        
        [JsonProperty(ModelNames.Speed)]
        public float Speed { get; set; }
        
        [JsonProperty(ModelNames.IntensityX)]
        public float IntensityX { get; set; }
        
        [JsonProperty(ModelNames.IntensityY)]
        public float IntensityY { get; set; }

        public Shake()
        {
            Intensity = 1;
            Speed = 1;
            IntensityX = 1;
            IntensityY = 1;
        }
        public Shake(int frame, EaseType ease, float intensity, float speed,
            float intensityX = 1f, float intensityY = 1f) : base(frame, ease)
        {
            Intensity = intensity;
            Speed = speed;
            IntensityX = intensityX;
            IntensityY = intensityY;
        }
    }
}