using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class Vector4Converter : JsonConverterCustomType<IVector4, VectorType>
    {
        public Vector4Converter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override VectorType GetCustomType(IVector4 value) => value.GetModelType();
        public override Type GetType(VectorType customType)
        {
            return customType switch
            {
                VectorType.Value => typeof(Vector4Value),
                VectorType.RandomRect => typeof(Vector4Rect),
                VectorType.RandomRectStep => typeof(Vector4RectStep),
                VectorType.RandomCircle => typeof(Vector4Circle),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}