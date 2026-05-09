using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Values.Vectors
{
    public class Vector4Circle : IVector4, ICopyable<Vector4Circle>
    {
        [JsonProperty(Names.CoordX)]
        public float X { get; set; }
        
        [JsonProperty(Names.CoordY)]
        public float Y { get; set; }
        
        [JsonProperty(Names.CoordZ)]
        public float Z { get; set; }
        
        [JsonProperty(Names.CoordW)]
        public float W { get; set; }
        
        [JsonProperty(Names.Radius)]
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

        public VectorType GetModelType() => VectorType.RandomCircle;

        IVector4 ICopyable<IVector4>.Copy() => new Vector4Circle(X, Y, Z, W, Radius);
        public Vector4Circle Copy() => new(X, Y, Z, W, Radius);
    }
}