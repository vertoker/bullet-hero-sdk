using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters.CustomTypes
{
    public class Vector3Converter : JsonConverterCustomType<IVector3, VectorType>
    {
        public Vector3Converter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override VectorType GetCustomType(IVector3 value) => value.GetModelType();
        public override Type GetType(VectorType customType)
        {
            return customType switch
            {
                VectorType.Value => typeof(Vector3Value),
                VectorType.RandomRect => typeof(Vector3Rect),
                VectorType.RandomRectStep => typeof(Vector3RectStep),
                VectorType.RandomCircle => typeof(Vector3Circle),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}