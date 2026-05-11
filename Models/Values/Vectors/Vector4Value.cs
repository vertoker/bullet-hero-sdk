using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
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

        public VectorType GetModelType() => VectorType.Value;
        
        IVector4 ICopyable<IVector4>.Copy() => new Vector4Value(X, Y, Z, W);
        public Vector4Value Copy() => new(X, Y, Z, W);
    }
}