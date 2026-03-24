using System.Collections.Generic;
using BHSDK.Models.Modifications;
using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class PrefabInstance
    {
        // In build process, this PrefabInstance converted to RuntimeInstance
        // 1. Loaded SourcePrefab to runtime model
        // 2. It uses fields and reflection in modifications for finding and replace value in runtime model
        // 3. Play it
        
        [JsonProperty(ModelNames.Prefab)]
        public Prefab SourcePrefab { get; set; }
        
        [JsonProperty(ModelNames.Modification)]
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