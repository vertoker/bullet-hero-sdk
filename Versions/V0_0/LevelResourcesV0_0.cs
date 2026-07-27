using System.Collections.Generic;
using Newtonsoft.Json;

namespace BH.SDK.Versions.V0_0
{
    // ReSharper disable once InconsistentNaming
    
    [DataVersion(DataDomains.LevelResources, 0, 0)]
    public class LevelResourcesV0_0
    {
        [JsonProperty("test_resources")]
        public Dictionary<int, object> Resources { get; set; }
    }
}