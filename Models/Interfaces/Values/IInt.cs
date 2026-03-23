using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IInt
    {
        public IntType GetModelType();
        public int Get();
    }
}