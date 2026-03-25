using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    public class StringValue : IString
    {
        [JsonProperty(ModelNames.String)]
        public string Value { get; set; }

        public StringValue()
        {
            Value = string.Empty;
        }
        public StringValue(string value)
        {
            Value = value;
        }

        public StringType GetModelType() => StringType.Value;
        public string Get() => Value;
    }
}