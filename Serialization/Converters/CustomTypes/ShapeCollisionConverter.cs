using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Shapes;
using BHSDK.Serialization.Converters.Base;
using Newtonsoft.Json;

namespace BHSDK.Serialization.Converters.CustomTypes
{
    public class ShapeCollisionConverter : JsonConverterCustomType<ICollisionShape, ShapeType>
    {
        public ShapeCollisionConverter(JsonSerializer serializerDefault) : base(serializerDefault)
        {
            
        }

        public override ShapeType GetCustomType(ICollisionShape value) => value.GetModelType();

        public override Type GetType(ShapeType customType)
        {
            return customType switch
            {
                ShapeType.Point => typeof(PointCollisionShape),
                ShapeType.Line => typeof(LineCollisionShape),
                ShapeType.Circle => typeof(CircleCollisionShape),
                ShapeType.Rectangle => typeof(RectangleCollisionShape),
                ShapeType.Triangle => typeof(TriangleCollisionShape),
                _ => throw new ArgumentOutOfRangeException(nameof(customType), customType, null)
            };
        }
    }
}