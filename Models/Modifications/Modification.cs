using Newtonsoft.Json;

namespace BHSDK.Models.Modifications
{
    public class Modification
    {
        [JsonProperty(ModelNames.Path)]
        public string PropertyPath { get; set; }
        
        [JsonProperty(ModelNames.Value)]
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