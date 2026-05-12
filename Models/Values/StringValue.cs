using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class StringValue : IString, ICopyable<StringValue>
    {
        [RuleNotNull, RuleStringMax(ValueRules.MaxGameString)]
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
        
        public object Clone() => Copy();
        IString ICopyable<IString>.Copy() => new StringValue(Value);
        public StringValue Copy() => new(Value);
    }
}