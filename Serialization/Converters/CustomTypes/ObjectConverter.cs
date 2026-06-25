using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Objects;
using BH.SDK.Serialization.Converters.Base;
using Newtonsoft.Json;
using Object = BH.SDK.Models.Objects.Object;
using Objects_Object = BH.SDK.Models.Objects.Object;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class ObjectConverter : JsonConverterCustomType<Objects_Object, ObjectType>
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
                ObjectType.TextureObject => typeof(TextureObject),
                ObjectType.TextObject => typeof(TextObject),
                ObjectType.EffectObject => typeof(EffectObject),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}