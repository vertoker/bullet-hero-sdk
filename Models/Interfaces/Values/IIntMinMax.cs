using BHSDK.Interfaces;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IIntMinMax : IInt, IModelReflection<IntMinMaxV1, IIntMinMax>
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }
}