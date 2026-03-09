using BHSDK.Interfaces;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IVectorCircle : IVector, IModelReflection<VectorCircleV1, IVectorCircle>
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }
    }
}