using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IVector4 : ICopyable<IVector4>
    {
        public VectorType GetModelType();
    }
}