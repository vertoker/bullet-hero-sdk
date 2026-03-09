using BHSDK.Interfaces;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IFloatValue : IFloat, IModelReflection<FloatValueV1, IFloatValue>
    {
        public float Value { get; set; }
    }
}