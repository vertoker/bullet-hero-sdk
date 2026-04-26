using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Shapes;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters.CustomTypes
{
    public class ShapeCollisionConverter : JsonConverterCustomType<IColliderShape, ColliderShapeType>
    {
        public ShapeCollisionConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override ColliderShapeType GetCustomType(IColliderShape value) => value.GetModelType();

        public override Type GetType(ColliderShapeType customType)
        {
            return customType switch
            {
                ColliderShapeType.Circle => typeof(CircleColliderShape),
                ColliderShapeType.Rectangle => typeof(RectangleColliderShape),
                ColliderShapeType.Triangle => typeof(TriangleColliderShape),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}