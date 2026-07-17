using System;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Enum.Effects;
using BH.SDK.Models.Interfaces.Effects;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class EffectColorConverter : JsonConverterCustomType<IEffectColor, EffectColorType>
    {
        public override EffectColorType GetCustomType(IEffectColor value) => value.GetModelType();
        public override Type GetType(EffectColorType customType)
        {
            return customType switch
            {
                EffectColorType.Value => typeof(EffectColorValue),
                EffectColorType.GradientOverLife => typeof(EffectColorGradientOverLife),
                EffectColorType.GradientBySpeed => typeof(EffectColorGradientBySpeed),
                EffectColorType.RandomUniform => typeof(EffectColorRandomUniform),
                EffectColorType.RandomPerComponent => typeof(EffectColorRandomPerComponent),
                EffectColorType.GradientRandom => typeof(EffectColorGradientRandom),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}