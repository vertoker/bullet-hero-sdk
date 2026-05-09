using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IVector2 : ICopyable<IVector2>
    {
        public VectorType GetModelType();
    }
}