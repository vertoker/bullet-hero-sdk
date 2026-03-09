using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IInt
    {
        public IntType Type { get; }
        public int Get();
    }
}