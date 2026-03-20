using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class Modification
    {
        [JsonProperty("pp")]
        public string PropertyPath { get; set; }
        
        [JsonProperty("v")]
        public string Value { get; set; }

        public Modification()
        {
            PropertyPath = string.Empty;
            Value = string.Empty;
        }
        public Modification(string propertyPath, string value)
        {
            PropertyPath = propertyPath;
            Value = value;
        }
    }
}