using System;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Serialization.Converters.Base;

namespace BH.SDK.Serialization.Converters.CustomTypes
{
    public class Vector2Converter : JsonConverterCustomType<IVector2, VectorType>
    {
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