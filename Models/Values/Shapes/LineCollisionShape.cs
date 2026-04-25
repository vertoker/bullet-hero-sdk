using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using Newtonsoft.Json;

namespace BHSDK.Models.Values.Shapes
{
    public class LineCollisionShape : ICollisionShape
    {
        [JsonProperty(Names.Point1)]
        public Vector2Value Point1 { get; set; }
        
        [JsonProperty(Names.Point2)]
        public Vector2Value Point2 { get; set; }
        
        public ShapeType GetModelType() => ShapeType.Line;

        public LineCollisionShape()
        {
            Point1 = new Vector2Value(0f, 0f);
            Point2 = new Vector2Value(1f, 0f);
        }
        public LineCollisionShape(float x1, float y1, float x2, float y2)
        {
            Point1 = new Vector2Value(x1, y1);
            Point2 = new Vector2Value(x2, y2);
        }
        public LineCollisionShape(Vector2Value point1, Vector2Value point2)
        {
            Point1 = point1;
            Point2 = point2;
        }
    }
}