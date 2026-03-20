using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class VectorValue : IVector
    {
        [JsonProperty("x")]
        public float X { get; set; }
        
        [JsonProperty("y")]
        public float Y { get; set; }

        public VectorValue()
        {
            X = 0f;
            Y = 0f;
        }
        public VectorValue(float x, float y)
        {
            X = x;
            Y = y;
        }
        public VectorValue(IFloat x, IFloat y)
        {
            X = x.Get();
            Y = y.Get();
        }

        public VectorType Type => VectorType.Value;
        public Vector2 Get() => new(X, Y);
    }
}