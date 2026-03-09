using BHSDK.Interfaces;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IIntMinMaxStep : IInt, IModelReflection<IntMinMaxStepV1, IIntMinMaxStep>
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Step { get; set; }
    }
}