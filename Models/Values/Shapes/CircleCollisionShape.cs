using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using Newtonsoft.Json;

namespace BHSDK.Models.Values.Shapes
{
    public class CircleCollisionShape : ICollisionShape
    {
        [JsonProperty(Names.CenterShort)]
        public Vector2Value Center { get; set; }
        
        [JsonProperty(Names.RadiusShort)]
        public float Radius { get; set; }
        
        public ShapeType GetModelType() => ShapeType.Circle;

        public CircleCollisionShape()
        {
            Center = new Vector2Value(0f, 0f);
            Radius = 1.0f;
        }
        public CircleCollisionShape(float x, float y, float radius)
        {
            Center = new Vector2Value(x, y);
            Radius = radius;
        }
        public CircleCollisionShape(Vector2Value center, float radius)
        {
            Center = center;
            Radius = radius;
        }
    }
}