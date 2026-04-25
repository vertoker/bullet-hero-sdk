using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters.CustomTypes
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