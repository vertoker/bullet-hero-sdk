using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class Shake : Keyframe, ICopyable<Shake>
    {
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }
        
        [JsonProperty(Names.Speed)]
        public float Speed { get; set; }
        
        [JsonProperty(Names.CoordX)]
        public float IntensityX { get; set; }
        
        [JsonProperty(Names.CoordY)]
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

        public Shake Copy() => new(Frame, Ease, Intensity, Speed, IntensityX, IntensityY);
    }
}