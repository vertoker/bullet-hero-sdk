using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Objects;
using BH.SDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class ObjectConverter : JsonConverterCustomType<RectObject, ObjectType>
    {
        public ObjectConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override ObjectType GetCustomType(RectObject value) => value.GetModelType();
        public override Type GetType(ObjectType customType)
        {
            return customType switch
            {
                ObjectType.RectObject => typeof(RectObject),
                ObjectType.TextureObject => typeof(TextureObject),
                ObjectType.TextObject => typeof(TextObject),
                ObjectType.EffectObject => typeof(EffectObject),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}