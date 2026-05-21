using System;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Enum.Effects;
using BH.SDK.Models.Interfaces.Effects;
using BH.SDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.CustomTypes
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