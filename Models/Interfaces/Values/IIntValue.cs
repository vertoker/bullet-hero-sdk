using BHSDK.Interfaces;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IIntValue : IInt, IModelReflection<IntValueV1, IIntValue>
    {
        public int Value { get; set; }
    }
}