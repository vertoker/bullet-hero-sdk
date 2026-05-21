using System;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Enum.Effects;
using BH.SDK.Models.Interfaces.Effects;
using BH.SDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class EffectShapeConverter : JsonConverterCustomType<IEffectShape, EffectShapeType>
    {
        public EffectShapeConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override EffectShapeType GetCustomType(IEffectShape value) => value.GetModelType();
        public override Type GetType(EffectShapeType customType)
        {
            return customType switch
            {
                EffectShapeType.Point => typeof(EffectShapePoint),
                EffectShapeType.Circle => typeof(EffectShapeCircle),
                EffectShapeType.Rectangle => typeof(EffectShapeRectangle),
                EffectShapeType.Line => typeof(EffectShapeLine),
                EffectShapeType.Cone => typeof(EffectShapeCone),
                EffectShapeType.Torus => typeof(EffectShapeTorus),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}