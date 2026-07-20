using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.Meta
{
    [RuleContainer]
    public class Author : IModel<Author>
    {
        [RuleNotNull(typeof(StringValue)), RuleIStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public IString Name { get; set; }
        
        [RuleNotNull, RuleStringMax(ValueRules.MaxUrl)]
        [JsonProperty(Names.Url)]
        public string Url { get; set; }
        
        // TODO add comment metadata

        public Author()
        {
            Name = new StringValue();
            Url = string.Empty;
        }
        public Author(IString name, string url)
        {
            Name = name;
            Url = url;
        }
        public void Reset()
        {
            Name = new StringValue();
            Url = string.Empty;
        }

        public object Clone() => Copy();
        public Author Copy() => new(Name.Copy(), Url);

        public override bool Equals(object obj) => obj is Author value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Name, Url);

        public bool Equals(Author other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Name.Equals(other.Name)
                         && Url.Equals(other.Url);
            return result;
        }
    }
}