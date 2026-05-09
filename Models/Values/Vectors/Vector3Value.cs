using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Values.Vectors
{
    public class Vector3Value : IVector3, ICopyable<Vector3Value>
    {
        [JsonProperty(Names.CoordX)]
        public float X { get; set; }
        
        [JsonProperty(Names.CoordY)]
        public float Y { get; set; }
        
        [JsonProperty(Names.CoordZ)]
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

        public VectorType GetModelType() => VectorType.Value;
        
        IVector3 ICopyable<IVector3>.Copy() => new Vector3Value(X, Y, Z);
        public Vector3Value Copy() => new(X, Y, Z);
    }
}