using Newtonsoft.Json;

namespace BHSDK.Models.Other
{
    public class InstancePropertyAddress
    {
        [JsonProperty("id")]
        public int InstanceId { get; set; }
        
        [JsonProperty("n")]
        public string Name { get; set; }
    }
}