using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IFloat
    {
        public FloatType Type { get; }
        public float Get();
    }
}