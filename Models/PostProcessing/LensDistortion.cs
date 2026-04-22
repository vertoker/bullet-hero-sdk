using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    public class LensDistortion : Keyframe
    {
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }
        
        [JsonProperty(Names.Multiplier)]
        public IVector2 Multiplier { get; set; }
        
        [JsonProperty(Names.Center)]
        public IVector2 Center { get; set; }
        
        [JsonProperty(Names.Scale)]
        public float Scale { get; set; }

        public LensDistortion()
        {
            Intensity = 0.5f;
            Multiplier = new Vector2Value(1f, 1f);
            Center = new Vector2Value(0.5f, 0.5f);
            Scale = 1f;
        }
        public LensDistortion(int frame, EaseType ease, 
            float intensity, IVector2 multiplier, IVector2 center, float scale)
            : base(frame, ease)
        {
            Intensity = intensity;
            Multiplier = multiplier;
            Center = center;
            Scale = scale;
        }
    }
}