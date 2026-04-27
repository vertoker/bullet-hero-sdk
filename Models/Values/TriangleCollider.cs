using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    public class TriangleCollider
    {
        [JsonProperty(Names.Point1)]
        public Vector2Value Point1 { get; set; }
        
        [JsonProperty(Names.Point2)]
        public Vector2Value Point2 { get; set; }
        
        [JsonProperty(Names.Point3)]
        public Vector2Value Point3 { get; set; }
        
        public TriangleCollider()
        {
            Point1 = new Vector2Value(-0.5f, -0.5f);
            Point2 = new Vector2Value(0.5f, -0.5f);
            Point3 = new Vector2Value(0.5f, 0.5f);
        }
        public TriangleCollider(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            Point1 = new Vector2Value(x1, y1);
            Point2 = new Vector2Value(x2, y2);
            Point3 = new Vector2Value(x3, y3);
        }
        public TriangleCollider(Vector2Value point1, Vector2Value point2, Vector2Value point3)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
        }
    }
}