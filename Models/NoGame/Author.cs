using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.NoGame
{
    public class Author
    {
        [JsonProperty(Names.Name)]
        public IString Name { get; set; }
        
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