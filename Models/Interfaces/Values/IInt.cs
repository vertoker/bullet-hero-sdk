using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IInt : ICopyable<IInt>
    {
        public IntType GetModelType();
        public int Get();
    }
}