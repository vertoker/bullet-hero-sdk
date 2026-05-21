using System;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Enum.Effects;
using BH.SDK.Models.Interfaces.Effects;
using BH.SDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class EffectShapeSpreadConverter : JsonConverterCustomType<IEffectShapeSpread, EffectShapeSpreadType>
    {
        public EffectShapeSpreadConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override EffectShapeSpreadType GetCustomType(IEffectShapeSpread value) => value.GetModelType();
        public override Type GetType(EffectShapeSpreadType customType)
        {
            return customType switch
            {
                EffectShapeSpreadType.Random => typeof(EffectShapeSpreadRandom),
                EffectShapeSpreadType.Loop => typeof(EffectShapeSpreadLoop),
                EffectShapeSpreadType.PingPong => typeof(EffectShapeSpreadPingPong),
                EffectShapeSpreadType.Sine => typeof(EffectShapeSpreadSine),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}