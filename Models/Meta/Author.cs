using System;
using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Meta
{
    [RuleContainer]
    public class Author : ICopyable<Author>, IEquatable<Author>
    {
        [RuleNotNull, RuleIStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public string Name { get; set; }
        
        [RuleInRange(ValueRules.MinAuthorOrder, ValueRules.MaxAuthorOrder)]
        [JsonProperty(Names.Order)]
        public int Order { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Url)]
        public string Url { get; set; }

        public Author()
        {
            Name = string.Empty;
            Order = 0;
            Url = string.Empty;
        }
        public Author(string name, int order, string url)
        {
            Name = name;
            Order = order;
            Url = url;
        }

        public object Clone() => Copy();
        public Author Copy() => new(Name, Order, Url);

        public override bool Equals(object obj) => obj is Author value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Name, Order, Url);

        public bool Equals(Author other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Name.Equals(other.Name)
                         && Order.Equals(other.Order)
                         && Url.Equals(other.Url);
            return result;
        }
    }
}