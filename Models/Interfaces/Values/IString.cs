using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IString : ICopyable<IString>
    {
        public StringType GetModelType();
        public string Get();
    }
}