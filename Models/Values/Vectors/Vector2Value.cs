using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values.Vectors
{
    public class Vector2Value : IVector2
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
        public Vector2Value(IFloat x, IFloat y)
        {
            X = x.Get();
            Y = y.Get();
        }
        public Vector2Value(Vector2 vector)
        {
            X = vector.x;
            Y = vector.y;
        }

        public VectorType GetModelType() => VectorType.Value;
        public Vector2 Get() => new(X, Y);
    }
}