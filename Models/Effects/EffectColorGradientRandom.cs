using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectColorGradientRandom : IEffectColor
    {
        [JsonProperty(ModelNames.Gradient)]
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
    }
}