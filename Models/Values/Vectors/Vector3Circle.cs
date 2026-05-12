using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class Vector3Circle : IVector3, ICopyable<Vector3Circle>
    {
        [JsonProperty(Names.CoordX)]
        public float X { get; set; }
        
        [JsonProperty(Names.CoordY)]
        public float Y { get; set; }
        
        [JsonProperty(Names.CoordZ)]
        public float Z { get; set; }
        
        [RuleMin(0f)]
        [JsonProperty(Names.Radius)]
        public float Radius { get; set; }
        
        public Vector3Circle()
        {
            X = 0f;
            Y = 0f;
            Z = 0f;
            Radius = 1f;
        }
        public Vector3Circle(float x, float y, float z, float radius)
        {
            X = x;
            Y = y;
            Z = z;
            Radius = radius;
        }

        public VectorType GetModelType() => VectorType.RandomCircle;

        public object Clone() => Copy();
        IVector3 ICopyable<IVector3>.Copy() => new Vector3Circle(X, Y, Z, Radius);
        public Vector3Circle Copy() => new(X, Y, Z, Radius);
    }
}