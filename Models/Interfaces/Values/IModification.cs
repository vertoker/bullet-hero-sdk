using BHSDK.Interfaces;
using BHSDK.Models.V1.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IModification : IModelReflection<ModificationV1, IModification>
    {
        public string PropertyPath { get; set; }
        public string Value { get; set; }
    }
}