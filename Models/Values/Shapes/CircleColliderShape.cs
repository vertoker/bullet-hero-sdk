using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using Newtonsoft.Json;

namespace BHSDK.Models.Values.Shapes
{
    public class CircleColliderShape : IColliderShape
    {
        [JsonProperty(Names.CenterShort)]
        public Vector2Value Center { get; set; }
        
        [JsonProperty(Names.RadiusShort)]
        public float Radius { get; set; }
        
        public ColliderShapeType GetModelType() => ColliderShapeType.Circle;

        public CircleColliderShape()
        {
            Center = new Vector2Value(0f, 0f);
            Radius = 1.0f;
        }
        public CircleColliderShape(float x, float y, float radius)
        {
            Center = new Vector2Value(x, y);
            Radius = radius;
        }
        public CircleColliderShape(Vector2Value center, float radius)
        {
            Center = center;
            Radius = radius;
        }
    }
}