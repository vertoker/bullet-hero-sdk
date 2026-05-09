using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    public class StringLanguage : ICopyable<StringLanguage>
    {
        [JsonProperty(Names.Language)]
        public string LanguageCode { get; set; }
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxGameString)]
        [JsonProperty(Names.ValueShort)]
        public string Value { get; set; }

        public StringLanguage()
        {
            LanguageCode = "en";
            Value = string.Empty;
        }
        public StringLanguage(string languageCode, string value)
        {
            LanguageCode = languageCode;
            Value = value;
        }

        public StringLanguage Copy() => new(LanguageCode, Value);
    }
}