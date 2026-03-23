using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    public class Vignette : Keyframe
    {
        [JsonProperty("c")]
        public IColor Color { get; set; } // TODO only RGB
        
        [JsonProperty("c")]
        public IVector2 Center { get; set; }
        
        [JsonProperty("i")]
        public float Intensity { get; set; }
        
        [JsonProperty("s")]
        public float Smoothness { get; set; }
        
        [JsonProperty("r")]
        public bool Rounded { get; set; }

        public Vignette()
        {
            Color = new ColorValue(UnityEngine.Color.black);
            Center = new Vector2Value(0.5f, 0.5f);
            Intensity = 0.3f;
            Smoothness = 0.5f;
            Rounded = false;
        }
        public Vignette(int frame, EaseType ease, IColor color, 
            IVector2 center, float intensity, float smoothness, bool rounded)
            : base(frame, ease)
        {
            Color = color;
            Center = center;
            Intensity = intensity;
            Smoothness = smoothness;
            Rounded = rounded;
        }
    }
}