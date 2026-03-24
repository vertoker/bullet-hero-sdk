using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BHSDK.Models.Values
{
    public class Vector4Circle : IVector4
    {
        [JsonProperty(ModelNames.CoordX)]
        public float X { get; set; }
        
        [JsonProperty(ModelNames.CoordY)]
        public float Y { get; set; }
        
        [JsonProperty(ModelNames.CoordZ)]
        public float Z { get; set; }
        
        [JsonProperty(ModelNames.CoordW)]
        public float W { get; set; }
        
        [JsonProperty(ModelNames.Radius)]
        public float Radius { get; set; }
        
        public Vector4Circle()
        {
            X = 0f;
            Y = 0f;
            Z = 0f;
            W = 0f;
            Radius = 1f;
        }
        public Vector4Circle(float x, float y, float z, float w, float radius)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
            Radius = radius;
        }
        public Vector4Circle(IFloat x, IFloat y, IFloat z, IFloat w, IFloat radius)
        {
            X = x.Get();
            Y = y.Get();
            Z = z.Get();
            W = w.Get();
            Radius = radius.Get();
        }

        public VectorType GetModelType() => VectorType.RandomCircle;
        public Vector4 Get()
        {
            // https://www.youtube.com/watch?v=4y_nmpv-9lI
            // Use distribution through Sqrt, because it uses one number.
            // Not optimal, but random predicted
            var distance = Mathf.Sqrt(Random.value);
            var angle = Random.Range(0f, 2f * Mathf.PI);

            var x = distance * Mathf.Cos(angle) * Radius + X;
            var y = distance * Mathf.Sin(angle) * Radius + Y;
            var z = distance * Mathf.Cos(angle) * Radius + Z;
            var w = distance * Mathf.Cos(angle) * Radius + W;
            return new Vector4(x, y, z, w);
        }
    }
}