using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters.CustomTypes
{
    public class Vector2Converter : JsonConverterCustomType<IVector2, VectorType>
    {
        public Vector2Converter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override VectorType GetCustomType(IVector2 value) => value.GetModelType();

        public override Type GetType(VectorType customType)
        {
            return customType switch
            {
                VectorType.Value => typeof(Vector2Value),
                VectorType.RandomRect => typeof(Vector2Rect),
                VectorType.RandomRectStep => typeof(Vector2RectStep),
                VectorType.RandomCircle => typeof(Vector2Circle),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}