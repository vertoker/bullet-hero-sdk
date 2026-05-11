using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.NoGame
{
    [RuleContainer]
    public class Author
    {
        [RuleNotNull(typeof(StringValue)), RuleIStringMax(ValueRules.MaxGameString)]
        [JsonProperty(Names.Name)]
        public IString Name { get; set; }
        
        [RuleInRange(-10000, 10000)]
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
    }
}