using System.Collections.Generic;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    public class PrefabObject
    {
        // In build process, this PrefabObject converted to Object
        // 1. Loaded SourcePrefab to runtime model
        // 2. It uses fields and reflection in modifications for finding and replace value in runtime model
        // 3. Play it
        
        [JsonProperty(Names.PrefabIndex)]
        public int PrefabIndex { get; set; } // reference to all level Prefabs list
        
        [JsonProperty(Names.Mod)]
        public List<Modification> Modifications { get; set; }

        public PrefabObject()
        {
            PrefabIndex = 0;
            Modifications = new List<Modification>();
        }
        public PrefabObject(int prefabIndex, List<Modification> modifications)
        {
            PrefabIndex = prefabIndex;
            Modifications = modifications;
        }
    }
}