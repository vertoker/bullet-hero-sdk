using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectColorGradientRandom : IEffectColor
    {
        [JsonProperty(ModelNames.Gradient)]
        public GradientValue Gradient { get; set; }

        public EffectColorType GetModelType() => EffectColorType.GradientRandom;

        public EffectColorGradientRandom()
        {
            Gradient = new GradientValue();
        }
        public EffectColorGradientRandom(GradientValue gradient)
        {
            Gradient = gradient;
        }
    }
}