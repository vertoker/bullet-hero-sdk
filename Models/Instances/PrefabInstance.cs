using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class PrefabInstance
    {
        [JsonProperty("sp")]
        public Prefab SourcePrefab { get; set; }
        
        [JsonProperty("ms")]
        public List<Modification> Modifications { get; set; }

        public PrefabInstance()
        {
            SourcePrefab = new Prefab();
            Modifications = new List<Modification>();
        }
        public PrefabInstance(Prefab sourcePrefab, List<Modification> modifications)
        {
            SourcePrefab = sourcePrefab;
            Modifications = modifications;
        }
    }
}