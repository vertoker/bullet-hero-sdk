using System;
using System.Collections.Generic;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    [RuleContainer]
    public class Prefab : IObjectScope, IPrefab, ICopyable<Prefab>
    {
        public Version GetVersion() => new(1, 0);
        
        [RuleGuidNotEmpty]
        [JsonProperty(Names.PrefabGuid)]
        public Guid PrefabGuid { get; set; }
        
        // TODO add more contextual checks
        [RuleNotNull]
        [RuleCollectionUnique(nameof(Object.ObjectId))]
        [JsonProperty(Names.Objects)]
        public List<Object> Objects { get; set; }
        
        // TODO add more contextual checks
        [RuleNotNull]
        [JsonProperty(Names.ParentObjects)]
        public List<PrefabObject> PrefabObjects { get; set; }

        public Prefab()
        {
            Objects = new List<Object>();
            PrefabObjects = new List<PrefabObject>();
        }
        public Prefab(List<Object> objects, List<PrefabObject> prefabObjects)
        {
            Objects = objects;
            PrefabObjects = prefabObjects;
        }

        public object Clone() => Copy();
        public Prefab Copy() => new(Objects.CopyList(), PrefabObjects.CopyList());
    }
}