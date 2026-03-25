using System;
using BHSDK.Models.Effects;
using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters.CustomTypes
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