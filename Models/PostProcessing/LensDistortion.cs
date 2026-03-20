using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    public class LensDistortion : Keyframe
    {
        [JsonProperty("i")]
        public float Intensity { get; set; }
        
        [JsonProperty("m")]
        public IVector Multiplier { get; set; }
        
        [JsonProperty("c")]
        public IVector Center { get; set; }
        
        [JsonProperty("s")]
        public float Scale { get; set; }

        public LensDistortion()
        {
            Intensity = 0.5f;
            Multiplier = new VectorValue(1f, 1f);
            Center = new VectorValue(0.5f, 0.5f);
            Scale = 1f;
        }
        public LensDistortion(int frame, EaseType ease, 
            float intensity, IVector multiplier, IVector center, float scale)
            : base(frame, ease)
        {
            Intensity = intensity;
            Multiplier = multiplier;
            Center = center;
            Scale = scale;
        }
    }
}