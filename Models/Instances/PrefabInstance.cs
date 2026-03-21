using System.Collections.Generic;
using BHSDK.Models.Other;
using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class PrefabInstance
    {
        // In build process, this PrefabInstance converted to RuntimeInstance
        // 1. Loaded SourcePrefab to runtime model
        // 2. It uses fields and reflection in modifications for finding and replace value in runtime model
        // 3. Play it
        
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