using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IFloat : ICopyable<IFloat>
    {
        public FloatType GetModelType();
        public float Get();
    }
}