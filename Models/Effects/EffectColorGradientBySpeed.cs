using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectColorGradientBySpeed : IEffectColor
    {
        [JsonProperty(ModelNames.Gradient)]
        public GradientValue Gradient { get; set; }
        
        [JsonProperty(ModelNames.Speed + ModelNames.Range)]
        public IVector2 SpeedRange { get; set; }

        public EffectColorType GetModelType() => EffectColorType.GradientBySpeed;

        public EffectColorGradientBySpeed()
        {
            Gradient = new GradientValue();
            SpeedRange = new Vector2Value(0f, 1f);
        }
        public EffectColorGradientBySpeed(GradientValue gradient, Vector2 speedRange)
        {
            Gradient = gradient;
            SpeedRange = new Vector2Value(speedRange);
        }
        public EffectColorGradientBySpeed(GradientValue gradient, IVector2 speedRange)
        {
            Gradient = gradient;
            SpeedRange = speedRange;
        }
    }
}