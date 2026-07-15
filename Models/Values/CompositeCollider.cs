using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class CompositeCollider : ICopyable<CompositeCollider>, IEquatable<CompositeCollider>
    {
        [RuleMax(IdRules.MaxUserColliderId)]
        [JsonProperty(Names.ColliderId)]
        public int ColliderId { get; set; }
        
        [JsonProperty(Names.ColliderName)]
        public string ColliderName { get; set; }
        
        // TODO (MAYBE) add Pivot for collider and maybe add it into collision process
        // TODO also most reason for it - extend game editor, because game colliders has it only for visuals
        // TODO or this can be PreferredPivot, it can be used by user in optional pivot in selection
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxColliderTriangles)]
        [JsonProperty(Names.TrianglesShort)]
        public List<TriangleCollider> Triangles { get; set; }

        public CompositeCollider()
        {
            ColliderId = IdRules.DefaultColliderId;
            ColliderName = string.Empty;
            Triangles = new List<TriangleCollider>();
        }
        public CompositeCollider(int colliderId, string colliderName, List<TriangleCollider> triangles)
        {
            ColliderId = colliderId;
            ColliderName = colliderName;
            Triangles = triangles;
        }

        public object Clone() => Copy();
        public CompositeCollider Copy() => new(ColliderId, ColliderName, Triangles.CopyList());

        public override bool Equals(object obj) => obj is CompositeCollider value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(ColliderId, ColliderName, Triangles.GetListHashCode());

        public bool Equals(CompositeCollider other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = ColliderId.Equals(other.ColliderId)
                         && ColliderName.Equals(other.ColliderName)
                         && Triangles.ListEquals(other.Triangles);
            return result;
        }
    }
}