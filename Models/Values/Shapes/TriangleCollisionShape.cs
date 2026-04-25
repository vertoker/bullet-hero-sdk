using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using Newtonsoft.Json;

namespace BHSDK.Models.Values.Shapes
{
    public class TriangleCollisionShape : ICollisionShape
    {
        [JsonProperty(Names.Point1)]
        public Vector2Value Point1 { get; set; }
        
        [JsonProperty(Names.Point2)]
        public Vector2Value Point2 { get; set; }
        
        [JsonProperty(Names.Point3)]
        public Vector2Value Point3 { get; set; }
        
        public ShapeType GetModelType() => ShapeType.Triangle;

        public TriangleCollisionShape()
        {
            Point1 = new Vector2Value(0f, 0f);
            Point2 = new Vector2Value(1f, 0f);
            Point3 = new Vector2Value(1f, 1f);
        }
        public TriangleCollisionShape(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            Point1 = new Vector2Value(x1, y1);
            Point2 = new Vector2Value(x2, y2);
            Point3 = new Vector2Value(x3, y3);
        }
        public TriangleCollisionShape(Vector2Value point1, Vector2Value point2, Vector2Value point3)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
        }
    }
}