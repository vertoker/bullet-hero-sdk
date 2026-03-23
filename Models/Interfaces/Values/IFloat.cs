using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IFloat
    {
        public FloatType GetModelType();
        public float Get();
    }
}