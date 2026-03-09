using BHSDK.Interfaces;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IVectorValue : IVector, IModelReflection<VectorValueV1, IVectorValue>
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}