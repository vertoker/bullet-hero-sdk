using Newtonsoft.Json;

namespace BHSDK.Models.NoGame
{
    public class Author
    {
        [JsonProperty(ModelNames.Name)]
        public string Name { get; set; }
        
        [JsonProperty(ModelNames.Order)]
        public int Order { get; set; }

        public Author()
        {
            Name = string.Empty;
            Order = 0;
        }
        public Author(string name, int order)
        {
            Name = name;
            Order = order;
        }
    }
}