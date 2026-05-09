using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Values.Vectors
{
    public class Vector2Value : IVector2, ICopyable<Vector2Value>
    {
        [JsonProperty(Names.CoordX)]
        public float X { get; set; }
        
        [JsonProperty(Names.CoordY)]
        public float Y { get; set; }

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
        
        IVector2 ICopyable<IVector2>.Copy() => new Vector2Value(X, Y);
        public Vector2Value Copy() => new(X, Y);
    }
}