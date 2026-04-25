using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using BHSDK.Models.Values.Vectors;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    public class Vignette : Keyframe
    {
        [JsonProperty(Names.Color)]
        public IColor ColorHDR { get; set; }
        
        [JsonProperty(Names.Center)]
        public IVector2 Center { get; set; }
        
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }
        
        [JsonProperty(Names.Smoothness)]
        public float Smoothness { get; set; }
        
        [JsonProperty(Names.Rounded)]
        public bool Rounded { get; set; }

        public Vignette()
        {
            ColorHDR = new ColorValue(UnityEngine.Color.black);
            Center = new Vector2Value(0.5f, 0.5f);
            Intensity = 0.3f;
            Smoothness = 0.5f;
            Rounded = false;
        }
        public Vignette(int frame, EaseType ease, IColor colorHDR, 
            IVector2 center, float intensity, float smoothness, bool rounded)
            : base(frame, ease)
        {
            ColorHDR = colorHDR;
            Center = center;
            Intensity = intensity;
            Smoothness = smoothness;
            Rounded = rounded;
        }
    }
}