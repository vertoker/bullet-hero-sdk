using System;
using BHSDK.Models.Effects;
using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters
{
    public class EffectAngleConverter : JsonConverterCustomType<IEffectAngle, EffectAngleType>
    {
        public EffectAngleConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override EffectAngleType GetCustomType(IEffectAngle value) => value.GetModelType();
        public override Type GetType(EffectAngleType customType)
        {
            return customType switch
            {
                EffectAngleType.Value => typeof(EffectAngleValue),
                EffectAngleType.CurvesOverLife => typeof(EffectAngleCurvesOverLife),
                EffectAngleType.CurvesBySpeed => typeof(EffectAngleCurvesBySpeed),
                EffectAngleType.RandomUniform => typeof(EffectAngleRandomUniform),
                EffectAngleType.RandomPerComponent => typeof(EffectAngleRandomPerComponent),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}