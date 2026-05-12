using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    [RuleContainer]
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
            Intensity = 1f;
            Speed = 1f;
            IntensityX = 1f;
            IntensityY = 1f;
        }
        public Shake(float intensity, float speed,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
            Speed = speed;
            IntensityX = 1f;
            IntensityY = 1f;
        }
        public Shake(float intensity, float speed, float intensityX, float intensityY,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
            Speed = speed;
            IntensityX = intensityX;
            IntensityY = intensityY;
        }

        public Shake Copy() => new(Intensity, Speed, IntensityX, IntensityY, Frame, Ease);
    }
}