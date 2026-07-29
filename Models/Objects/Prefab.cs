using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using BH.SDK.Versions;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    [RuleContainer]
    [DataVersion(DataDomains.Prefab, 1, 0)]
    public class Prefab : IObjectScope, IModel<Prefab>
    {
        [RuleIPrimitiveGuidNotNull]
        [JsonProperty(Names.PrefabId)]
        public PrefabId PrefabId { get; set; }
        
        // TODO add a contextual Rule validating this whole dictionary (key must equal value's own ObjectId)
        [RuleNotNull]
        [JsonProperty(Names.Objects)]
        public Dictionary<ObjectId, RectObject> Objects { get; set; }
        
        // TODO add more contextual checks
        [RuleNotNull]
        [JsonProperty(Names.ParentObjects)]
        public List<PrefabObject> PrefabObjects { get; set; }

        public Prefab()
        {
            PrefabId = PrefabId.Null;
            Objects = new Dictionary<ObjectId, RectObject>();
            PrefabObjects = new List<PrefabObject>();
        }
        public Prefab(PrefabId prefabId, Dictionary<ObjectId, RectObject> objects, List<PrefabObject> prefabObjects)
        {
            PrefabId = prefabId;
            Objects = objects;
            PrefabObjects = prefabObjects;
        }
        public void Reset()
        {
            PrefabId = PrefabId.Null;
            Objects.Clear();
            PrefabObjects.Clear();
        }

        public object Clone() => Copy();
        public Prefab Copy() => new(PrefabId, Objects.CopyDictionary(), PrefabObjects.CopyList());

        public override bool Equals(object obj) => obj is Prefab value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(PrefabId,
            Objects.GetDictionaryHashCode(), PrefabObjects.GetListHashCode());

        public bool Equals(Prefab other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = PrefabId.Equals(other.PrefabId)
                         && Objects.DictionaryEquals(other.Objects)
                         && PrefabObjects.ListEquals(other.PrefabObjects);
            return result;
        }
    }
}