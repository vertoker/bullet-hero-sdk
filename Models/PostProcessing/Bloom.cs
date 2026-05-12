using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class Bloom : Keyframe, ICopyable<Bloom> // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
    {
        // Threshold - 0 (always, not a parameter)
        
        [RuleInRange(PostProcessingRules.Bloom.IntensityMin,
           PostProcessingRules.Bloom.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }
        
        [RuleInRange(PostProcessingRules.Bloom.ScatterMin,
            PostProcessingRules.Bloom.ScatterMax)]
        [JsonProperty(Names.Scatter)]
        public float Scatter { get; set; }
        
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.Color)]
        public IColor Color { get; set; }
        
        // Filter (player choose in settings: high - Gaussian, mid - Dual, low - Kawase)

        public Bloom()
        {
            Intensity = 0.5f;
            Scatter = 0.5f;
            Color = ColorValue.red;
        }
        public Bloom(float intensity, float scatter, IColor color,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
            Scatter = scatter;
            Color = color;
        }

        public object Clone() => Copy();
        public Bloom Copy() => new(Intensity, Scatter, Color.Copy(), Frame, Ease);
    }
}