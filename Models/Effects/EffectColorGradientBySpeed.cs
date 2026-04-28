using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectColorGradientBySpeed : IEffectColor, ICopyable<EffectColorGradientBySpeed>
    {
        [JsonProperty(Names.Gradient)]
        public GradientValue Gradient { get; set; }
        
        [JsonProperty(Names.SpeedRange)]
        public IVector2 SpeedRange { get; set; }

        public EffectColorType GetModelType() => EffectColorType.GradientBySpeed;

        public EffectColorGradientBySpeed()
        {
            Gradient = new GradientValue(EffectStatic.GetDefaultGradient());
            SpeedRange = new Vector2Value(EffectStatic.Color_BySpeedRangeDefault);
        }
        public EffectColorGradientBySpeed(Gradient gradient, Vector2 speedRange)
        {
            Gradient = new GradientValue(gradient);
            SpeedRange = new Vector2Value(speedRange);
        }
        public EffectColorGradientBySpeed(GradientValue gradient, IVector2 speedRange)
        {
            Gradient = gradient;
            SpeedRange = speedRange;
        }

        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorGradientBySpeed(Gradient.Copy(), SpeedRange.Copy());
        public EffectColorGradientBySpeed Copy() => new(Gradient.Copy(), SpeedRange.Copy());
    }
}