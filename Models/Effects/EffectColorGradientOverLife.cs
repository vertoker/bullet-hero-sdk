using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectColorGradientOverLife : IEffectColor
    {
        [JsonProperty(ModelNames.Gradient)]
        public GradientValue Gradient { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.GradientOverLife;

        public EffectColorGradientOverLife()
        {
            Gradient = new GradientValue();
        }
        public EffectColorGradientOverLife(GradientValue gradient)
        {
            Gradient = gradient;
        }
    }
}