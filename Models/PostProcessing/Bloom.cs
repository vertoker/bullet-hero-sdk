using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    public class Bloom : Keyframe // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
    {
        // Threshold - 0 (always, not a parameter)
        
        [JsonProperty(ModelNames.Intensity)]
        public float Intensity { get; set; }
        
        [JsonProperty(ModelNames.Scatter)]
        public float Scatter { get; set; }
        
        [JsonProperty(ModelNames.Color)]
        public IColor Color { get; set; }
        
        // Filter (player choose in settings: high - Gaussian, mid - Dual, low - Kawase)

        public Bloom()
        {
            Intensity = 0.5f;
            Scatter = 0.5f;
            Color = new ColorValue(UnityEngine.Color.red);
        }
        public Bloom(int frame, EaseType ease, 
            float intensity, float scatter, IColor color)
            : base(frame, ease)
        {
            Intensity = intensity;
            Scatter = scatter;
            Color = color;
        }
    }
}