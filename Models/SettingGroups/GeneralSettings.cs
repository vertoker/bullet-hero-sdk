using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.SettingGroups
{
    [RuleContainer]
    public class GeneralSettings
    {
        [RuleInRange(1, 8)]
        [JsonProperty(Names.ResourceParallelLoadCount)]
        public int ResourceParallelLoadCount { get; set; }
        
        [RuleMin(0f)]
        [JsonProperty(Names.ResourceParallelLoadCount)]
        public float ResourceWebTimeout { get; set; }

        public GeneralSettings()
        {
            ResourceParallelLoadCount = 2;
            ResourceWebTimeout = 5f;
        }
    }
}