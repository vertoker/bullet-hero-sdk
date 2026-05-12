using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class Vector2Value : IVector2, ICopyable<Vector2Value>
    {
        [JsonProperty(Names.CoordX)]
        public float X { get; set; }
        
        [JsonProperty(Names.CoordY)]
        public float Y { get; set; }

        public static Vector2Value Zero => new(0.0f, 0.0f);
        public static Vector2Value One => new(1.0f, 1.0f);
        
        public static Vector2Value Right => new(1.0f, 0.0f);
        public static Vector2Value Left => new(-1.0f, 0.0f);
        public static Vector2Value Up => new(0.0f, 1.0f);
        public static Vector2Value Down => new(0.0f, -1.0f);
        
        
        public Vector2Value()
        {
            X = 0f;
            Y = 0f;
        }
        public Vector2Value(float x, float y)
        {
            X = x;
            Y = y;
        }

        public VectorType GetModelType() => VectorType.Value;
        
        public object Clone() => Copy();
        IVector2 ICopyable<IVector2>.Copy() => new Vector2Value(X, Y);
        public Vector2Value Copy() => new(X, Y);
    }
}