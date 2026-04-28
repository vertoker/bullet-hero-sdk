using System;
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
        
        [JsonProperty(Names.PrefabGuid)]
        public Guid PrefabGuid { get; set; } // reference to all level Prefabs list
        
        [JsonProperty(Names.ObjectIds)]
        public List<ObjectIdModification> ObjectIds { get; set; }
        
        [JsonProperty(Names.Mod)]
        public List<Modification> Modifications { get; set; }

        public PrefabObject()
        {
            PrefabGuid = Guid.Empty;
            ObjectIds = new List<ObjectIdModification>();
            Modifications = new List<Modification>();
        }
        public PrefabObject(Guid prefabGuid, List<ObjectIdModification> objectIds, List<Modification> modifications)
        {
            PrefabGuid = prefabGuid;
            ObjectIds = objectIds;
            Modifications = modifications;
        }
    }
}