using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    [RuleContainer]
    public class EffectColorGradientOverLife : IEffectColor, ICopyable<EffectColorGradientOverLife>
    {
        [RuleNotNull]
        [JsonProperty(Names.Gradient)]
        public GradientValue Gradient { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.GradientOverLife;

        public EffectColorGradientOverLife()
        {
            Gradient = EffectRules.GetGradient_Default();
        }
        public EffectColorGradientOverLife(GradientValue gradient)
        {
            Gradient = gradient;
        }

        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorGradientOverLife(Gradient.Copy());
        public EffectColorGradientOverLife Copy() => new(Gradient.Copy());
    }
}