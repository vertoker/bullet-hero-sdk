using System;
using BHSDK.Models.Enum;
using BHSDK.Models.Objects;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;
using Object = BHSDK.Models.Base.Object;

namespace BHSDK.Serialization.Converters.CustomTypes
{
    public class ObjectConverter : JsonConverterCustomType<Object, ObjectType>
    {
        public ObjectConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override ObjectType GetCustomType(Object value) => value.GetModelType();
        public override Type GetType(ObjectType customType)
        {
            return customType switch
            {
                ObjectType.Object => typeof(Object),
                ObjectType.Texture => typeof(TextureObject),
                ObjectType.Text => typeof(TextObject),
                ObjectType.Effect => typeof(EffectObject),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}