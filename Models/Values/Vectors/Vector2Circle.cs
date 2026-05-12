using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class Vector2Circle : IVector2, ICopyable<Vector2Circle>
    {
        [JsonProperty(Names.CoordX)]
        public float X { get; set; }
        
        [JsonProperty(Names.CoordY)]
        public float Y { get; set; }
        
        [RuleMin(0f)]
        [JsonProperty(Names.Radius)]
        public float Radius { get; set; }
        
        public Vector2Circle()
        {
            X = 0f;
            Y = 0f;
            Radius = 1f;
        }
        public Vector2Circle(float x, float y, float radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public VectorType GetModelType() => VectorType.RandomCircle;
        
        public object Clone() => Copy();
        IVector2 ICopyable<IVector2>.Copy() => new Vector2Circle(X, Y, Radius);
        public Vector2Circle Copy() => new(X, Y, Radius);
    }
}