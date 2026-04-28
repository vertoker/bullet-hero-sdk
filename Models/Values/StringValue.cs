using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    public class StringValue : IString, ICopyable<StringValue>
    {
        [JsonProperty(Names.ValueShort)]
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
        
        IString ICopyable<IString>.Copy() => new StringValue(Value);
        public StringValue Copy() => new(Value);
    }
}