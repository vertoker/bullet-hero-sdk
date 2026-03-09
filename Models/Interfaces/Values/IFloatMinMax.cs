using BHSDK.Interfaces;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IFloatMinMax : IFloat, IModelReflection<FloatMinMaxV1, IFloatMinMax>
    {
        public float Min { get; set; }
        public float Max { get; set; }
    }
}