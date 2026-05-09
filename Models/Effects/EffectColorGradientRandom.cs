using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectColorGradientRandom : IEffectColor, ICopyable<EffectColorGradientRandom>
    {
        [JsonProperty(Names.Gradient)]
        public GradientValue Gradient { get; set; }

        public EffectColorType GetModelType() => EffectColorType.GradientRandom;

        public EffectColorGradientRandom()
        {
            Gradient = EffectStatic.GetGradient_Default();
        }
        public EffectColorGradientRandom(GradientValue gradient)
        {
            Gradient = gradient;
        }

        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorGradientRandom(Gradient.Copy());
        public EffectColorGradientRandom Copy() => new(Gradient.Copy());
    }
}