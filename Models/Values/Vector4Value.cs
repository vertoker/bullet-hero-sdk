using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class Vector4Value : IVector4
    {
        [JsonProperty("x")]
        public float X { get; set; }
        
        [JsonProperty("y")]
        public float Y { get; set; }
        
        [JsonProperty("z")]
        public float Z { get; set; }
        
        [JsonProperty("w")]
        public float W { get; set; }

        public Vector4Value()
        {
            X = 0f;
            Y = 0f;
            Z = 0f;
            W = 0f;
        }
        public Vector4Value(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Vector4Value(IFloat x, IFloat y, IFloat z, IFloat w)
        {
            X = x.Get();
            Y = y.Get();
            Z = z.Get();
            W = w.Get();
        }

        public VectorType Type => VectorType.Value;
        public Vector4 Get() => new(X, Y, Z, W);
    }
}