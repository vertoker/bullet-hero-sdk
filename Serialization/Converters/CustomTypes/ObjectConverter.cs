using System;
using BH.SDK.Models.Enums;
using BH.SDK.Models.Objects;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class ObjectConverter : JsonConverterCustomType<RectObject, ObjectType>
    {
        public override ObjectType GetCustomType(RectObject value) => value.GetModelType();
        public override Type GetType(ObjectType customType)
        {
            return customType switch
            {
                ObjectType.RectObject => typeof(RectObject),
                ObjectType.ShapeObject => typeof(ShapeObject),
                ObjectType.TextObject => typeof(TextObject),
                ObjectType.EffectObject => typeof(EffectObject),
                ObjectType.PrefabObject => typeof(PrefabObject),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}