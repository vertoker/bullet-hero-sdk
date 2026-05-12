using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.NoGame
{
    [RuleContainer]
    public class Author : ICopyable<Author>
    {
        [RuleNotNull(typeof(StringValue)), RuleIStringMax(ValueRules.MaxEditorName)]
        [JsonProperty(Names.Name)]
        public IString Name { get; set; }
        
        [RuleInRange(ValueRules.MinAuthorOrder, ValueRules.MaxAuthorOrder)]
        [JsonProperty(Names.Order)]
        public int Order { get; set; }

        public Author()
        {
            Name = new StringValue();
            Order = 0;
        }
        public Author(string name, int order)
        {
            Name = new StringValue(name);
            Order = order;
        }
        public Author(IString name, int order)
        {
            Name = name;
            Order = order;
        }

        public object Clone() => Copy();
        public Author Copy() => new(Name.Copy(), Order);
    }
}