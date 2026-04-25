using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using Newtonsoft.Json;

namespace BHSDK.Models.Values.Shapes
{
    public class RectangleCollisionShape : ICollisionShape
    {
        [JsonProperty(Names.CenterShort)]
        public Vector2Value Center { get; set; }
        
        [JsonProperty(Names.WidthShort)]
        public float Width { get; set; }
        
        [JsonProperty(Names.HeightShort)]
        public float Height { get; set; }
        
        public ShapeType GetModelType() => ShapeType.Rectangle;

        public RectangleCollisionShape()
        {
            Center = new Vector2Value(0f, 0f);
            Width = 1f;
            Height = 1f;
        }
        public RectangleCollisionShape(float x, float y, float width, float height)
        {
            Center = new Vector2Value(x, y);
            Width = width;
            Height = height;
        }
        public RectangleCollisionShape(Vector2Value center, float width, float height)
        {
            Center = center;
            Width = width;
            Height = height;
        }
    }
}