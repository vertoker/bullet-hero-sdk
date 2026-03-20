using Newtonsoft.Json;

namespace BHSDK.Models
{
    public class Author
    {
        [JsonProperty("n")]
        public string Name { get; set; }
        
        [JsonProperty("o")]
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