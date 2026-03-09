using BHSDK.Interfaces;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IFloatMinMaxStep : IFloat, IModelReflection<FloatMinMaxStepV1, IFloatMinMaxStep>
    {
        public float Min { get; set; }
        public float Max { get; set; }
        public float Step { get; set; }
    }
}