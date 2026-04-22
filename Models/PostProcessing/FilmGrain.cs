using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    public class FilmGrain : Keyframe
    {
        [JsonProperty(Names.Type)]
        public FilmGrainType Type { get; set; }
        
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }

        public FilmGrain()
        {
            Type = FilmGrainType.Medium1;
            Intensity = 1.0f;
        }
        public FilmGrain(int frame, EaseType ease, 
            FilmGrainType type, float intensity)
            : base(frame, ease)
        {
            Type = type;
            Intensity = intensity;
        }
    }
}