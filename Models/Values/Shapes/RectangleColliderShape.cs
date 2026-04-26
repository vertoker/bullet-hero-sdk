using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using Newtonsoft.Json;

namespace BHSDK.Models.Values.Shapes
{
    public class RectangleColliderShape : IColliderShape
    {
        [JsonProperty(Names.CenterShort)]
        public Vector2Value Center { get; set; }
        
        [JsonProperty(Names.WidthShort)]
        public float Width { get; set; }
        
        [JsonProperty(Names.HeightShort)]
        public float Height { get; set; }
        
        [JsonProperty(Names.Angle)]
        public float Angle { get; set; }
        
        public ColliderShapeType GetModelType() => ColliderShapeType.Rectangle;

        public RectangleColliderShape()
        {
            Center = new Vector2Value(0f, 0f);
            Width = 1f;
            Height = 1f;
            Angle = 0f;
        }
        public RectangleColliderShape(float x, float y, float width, float height, float angle)
        {
            Center = new Vector2Value(x, y);
            Width = width;
            Height = height;
            Angle = angle;
        }
        public RectangleColliderShape(Vector2Value center, float width, float height, float angle)
        {
            Center = center;
            Width = width;
            Height = height;
            Angle = angle;
        }
    }
}