using System;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Enum.Effects;
using BH.SDK.Models.Interfaces.Effects;
using BH.SDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class EffectScaleConverter : JsonConverterCustomType<IEffectScale, EffectScaleType>
    {
        public EffectScaleConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override EffectScaleType GetCustomType(IEffectScale value) => value.GetModelType();
        public override Type GetType(EffectScaleType customType)
        {
            return customType switch
            {
                EffectScaleType.Value => typeof(EffectScaleValue),
                EffectScaleType.CurvesOverLife => typeof(EffectScaleCurvesOverLife),
                EffectScaleType.CurvesBySpeed => typeof(EffectScaleCurvesBySpeed),
                EffectScaleType.RandomUniform => typeof(EffectScaleRandomUniform),
                EffectScaleType.RandomPerComponent => typeof(EffectScaleRandomPerComponent),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}