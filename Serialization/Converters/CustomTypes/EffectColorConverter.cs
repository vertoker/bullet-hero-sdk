using System;
using BHSDK.Models.Effects;
using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters.CustomTypes
{
    public class EffectColorConverter : JsonConverterCustomType<IEffectColor, EffectColorType>
    {
        public EffectColorConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

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