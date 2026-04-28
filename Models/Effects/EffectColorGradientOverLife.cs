using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectColorGradientOverLife : IEffectColor, ICopyable<EffectColorGradientOverLife>
    {
        [JsonProperty(Names.Gradient)]
        public GradientValue Gradient { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.GradientOverLife;

        public EffectColorGradientOverLife()
        {
            Gradient = new GradientValue(EffectStatic.GetDefaultGradient());
        }
        public EffectColorGradientOverLife(Gradient gradient)
        {
            Gradient = new GradientValue(gradient);
        }
        public EffectColorGradientOverLife(GradientValue gradient)
        {
            Gradient = gradient;
        }

        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorGradientOverLife(Gradient.Copy());
        public EffectColorGradientOverLife Copy() => new(Gradient.Copy());
    }
}