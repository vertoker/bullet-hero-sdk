using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectColorGradientRandom : IEffectColor, ICopyable<EffectColorGradientRandom>
    {
        [JsonProperty(Names.Gradient)]
        public GradientValue Gradient { get; set; }

        public EffectColorType GetModelType() => EffectColorType.GradientRandom;

        public EffectColorGradientRandom()
        {
            Gradient = new GradientValue(EffectStatic.GetDefaultGradient());
        }
        public EffectColorGradientRandom(Gradient gradient)
        {
            Gradient = new GradientValue(gradient);
        }
        public EffectColorGradientRandom(GradientValue gradient)
        {
            Gradient = gradient;
        }

        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorGradientRandom(Gradient.Copy());
        public EffectColorGradientRandom Copy() => new(Gradient.Copy());
    }
}