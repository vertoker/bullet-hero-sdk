using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class StringLanguage : ICopyable<StringLanguage>, IEquatable<StringLanguage>
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

        public object Clone() => Copy();
        public StringLanguage Copy() => new(LanguageCode, Value);

        public bool Equals(StringLanguage other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = LanguageCode == other.LanguageCode
                         && Value.Equals(other.Value);
            return result;
        }

        public override bool Equals(object obj) => obj is StringLanguage value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(LanguageCode, Value);
    }
}