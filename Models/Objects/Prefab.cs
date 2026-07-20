using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    [RuleContainer]
    public class Prefab : IObjectScope, IPrefab, IModel<Prefab>
    {
        public static readonly Version Version = new(1, 0);
        public Version GetVersion() => Version;
        
        [JsonProperty(Names.PrefabId)]
        public PrefabId PrefabId { get; set; }
        
        // TODO add more contextual checks
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