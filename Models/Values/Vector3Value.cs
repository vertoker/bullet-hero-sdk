using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class Vector3Value : IVector3
    {
        [JsonProperty(ModelNames.CoordX)]
        public float X { get; set; }
        
        [JsonProperty(ModelNames.CoordY)]
        public float Y { get; set; }
        
        [JsonProperty(ModelNames.CoordZ)]
        public float Z { get; set; }

        public Vector3Value()
        {
            X = 0f;
            Y = 0f;
            Z = 0f;
        }
        public Vector3Value(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3Value(IFloat x, IFloat y, IFloat z)
        {
            X = x.Get();
            Y = y.Get();
            Z = z.Get();
        }
        public Vector3Value(Vector3 vector)
        {
            X = vector.x;
            Y = vector.y;
            Z = vector.z;
        }

        public VectorType GetModelType() => VectorType.Value;
        public Vector3 Get() => new(X, Y, Z);
    }
}