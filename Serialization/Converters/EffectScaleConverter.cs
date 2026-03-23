using System;
using BHSDK.Models.Effects;
using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters
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