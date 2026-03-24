using Newtonsoft.Json;

namespace BHSDK.Models.Modifications
{
    public class InstancePropertyAddress
    {
        [JsonProperty(ModelNames.InstanceId)]
        public int InstanceId { get; set; }
        
        [JsonProperty(ModelNames.Name)]
        public string Name { get; set; }
    }
}