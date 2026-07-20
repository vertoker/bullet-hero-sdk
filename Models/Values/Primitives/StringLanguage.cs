using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class StringLanguage : IModel<StringLanguage>
    {
        [JsonProperty(Names.Language)]
        public string LanguageCode { get; set; }
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxGameString)]
        [JsonProperty(Names.ValueShort)]
        public string Value { get; set; }

        public StringLanguage()
        {
            LanguageCode = ValueRules.DefaultLanguageCode;
            Value = string.Empty;
        }
        public StringLanguage(string languageCode, string value)
        {
            LanguageCode = languageCode;
            Value = value;
        }
        public void Reset()
        {
            LanguageCode = ValueRules.DefaultLanguageCode;
            Value = string.Empty;
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