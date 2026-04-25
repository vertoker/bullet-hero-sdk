using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using Newtonsoft.Json;

namespace BHSDK.Models.Values.Shapes
{
    public class PointCollisionShape : ICollisionShape
    {
        [JsonProperty(Names.ValueShort)]
        public Vector2Value Value { get; set; }
        
        public ShapeType GetModelType() => ShapeType.Point;

        public PointCollisionShape()
        {
            Value = new Vector2Value(0f, 0f);
        }
        public PointCollisionShape(float x, float y)
        {
            Value = new Vector2Value(x, y);
        }
        public PointCollisionShape(Vector2Value value)
        {
            Value = value;
        }
    }
}