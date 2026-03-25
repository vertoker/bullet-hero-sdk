using System;
using BHSDK.Models.Effects;
using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters.CustomTypes
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