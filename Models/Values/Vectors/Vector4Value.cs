using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values.Vectors
{
    public class Vector4Value : IVector4, ICopyable<Vector4Value>
    {
        [JsonProperty(Names.CoordX)]
        public float X { get; set; }
        
        [JsonProperty(Names.CoordY)]
        public float Y { get; set; }
        
        [JsonProperty(Names.CoordZ)]
        public float Z { get; set; }
        
        [JsonProperty(Names.CoordW)]
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
        public Vector4Value(Vector4 vector)
        {
            X = vector.x;
            Y = vector.y;
            Z = vector.z;
            W = vector.w;
        }

        public VectorType GetModelType() => VectorType.Value;
        public Vector4 Get() => new(X, Y, Z, W);
        
        IVector4 ICopyable<IVector4>.Copy() => new Vector4Value(X, Y, Z, W);
        public Vector4Value Copy() => new(X, Y, Z, W);
    }
}