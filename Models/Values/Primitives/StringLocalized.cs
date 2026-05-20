using System;
using System.Collections.Generic;
using System.Linq;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class StringLocalized : IString, ICopyable<StringLocalized>, IEquatable<StringLocalized>
    {
        [RuleNotNull, RuleCollectionUnique(nameof(StringLanguage.LanguageCode))]
        [JsonProperty(Names.Strings)]
        public List<StringLanguage> Strings { get; set; }

        public StringLocalized()
        {
            Strings = new List<StringLanguage>();
        }
        public StringLocalized(params StringLanguage[] strings)
        {
            Strings = strings.ToList();
        }
        public StringLocalized(List<StringLanguage> strings)
        {
            Strings = strings;
        }

        public StringType GetModelType() => StringType.Localized;

        public object Clone() => Copy();
        IString ICopyable<IString>.Copy() => Copy();
        public StringLocalized Copy() => new(Strings.CopyList());
        
        public override bool Equals(object obj) => obj is StringLocalized value && Equals(value);
        public override int GetHashCode() => Strings.GetListHashCode();
        
        public bool Equals(IString other) => other is StringLocalized value && Equals(value);
        public bool Equals(StringLocalized other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Strings.ListEquals(other.Strings);
            return result;
        }
    }
}