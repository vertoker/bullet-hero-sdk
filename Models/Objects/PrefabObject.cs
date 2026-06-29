using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    [RuleContainer]
    public class PrefabObject : ICopyable<PrefabObject>, IEquatable<PrefabObject>
    {
        // In build process, this PrefabObject converted to RectObject
        // 1. Loaded SourcePrefab to runtime model
        // 2. It uses fields and reflection in modifications for finding and replace value in runtime model
        // 3. Play it
        
        [RuleGuidNotEmpty]
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

        public object Clone() => Copy();
        public PrefabObject Copy() => new(PrefabGuid, ObjectIds.CopyList(), Modifications.CopyList());

        public override bool Equals(object obj) => obj is PrefabObject value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(PrefabGuid,
            ObjectIds.GetListHashCode(), Modifications.GetListHashCode());

        public bool Equals(PrefabObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = PrefabGuid.Equals(other.PrefabGuid)
                         && ObjectIds.ListEquals(other.ObjectIds)
                         && Modifications.ListEquals(other.Modifications);
            return result;
        }
    }
}