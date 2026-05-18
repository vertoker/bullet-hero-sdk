using System;
using System.Collections.Generic;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Objects
{
    [RuleContainer]
    public class Prefab : IObjectScope, IPrefab, ICopyable<Prefab>, IEquatable<Prefab>
    {
        public static readonly Version Version = new(1, 0);
        public Version GetVersion() => Version;
        
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
            PrefabGuid = Guid.NewGuid();
            Objects = new List<Object>();
            PrefabObjects = new List<PrefabObject>();
        }
        public Prefab(Guid prefabGuid, List<Object> objects, List<PrefabObject> prefabObjects)
        {
            PrefabGuid = prefabGuid;
            Objects = objects;
            PrefabObjects = prefabObjects;
        }

        public object Clone() => Copy();
        public Prefab Copy() => new(PrefabGuid, Objects.CopyList(), PrefabObjects.CopyList());

        public override bool Equals(object obj) => obj is Prefab value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(PrefabGuid,
            Objects.GetListHashCode(), PrefabObjects.GetListHashCode());

        public bool Equals(Prefab other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = PrefabGuid.Equals(other.PrefabGuid)
                         && Objects.ListEquals(other.Objects)
                         && PrefabObjects.ListEquals(other.PrefabObjects);
            return result;
        }
    }
}