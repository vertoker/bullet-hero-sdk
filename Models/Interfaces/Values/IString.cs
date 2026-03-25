using BHSDK.Models.Enum.Values;

namespace BHSDK.Models.Interfaces.Values
{
    public interface IString
    {
        public StringType GetModelType();
        public string Get();
    }
}