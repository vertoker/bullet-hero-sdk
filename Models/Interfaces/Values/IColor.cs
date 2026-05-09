using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IColor : ICopyable<IColor>
    {
        public ColorType GetModelType();
    }
}