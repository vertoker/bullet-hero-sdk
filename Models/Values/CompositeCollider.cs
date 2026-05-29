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
        [RuleMax(IdRules.MaxColliderId)]
        [JsonProperty(Names.ColliderId)]
        public int ColliderId { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxColliderTriangles)]
        [JsonProperty(Names.TrianglesShort)]
        public List<TriangleCollider> Triangles { get; set; }

        public CompositeCollider()
        {
            ColliderId = IdRules.NullColliderId;
            Triangles = new List<TriangleCollider>();
        }
        public CompositeCollider(int colliderId, List<TriangleCollider> triangles)
        {
            ColliderId = colliderId;
            Triangles = triangles;
        }

        public object Clone() => Copy();
        public CompositeCollider Copy() => new(ColliderId, Triangles.CopyList());

        public override bool Equals(object obj) => obj is CompositeCollider value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(ColliderId, Triangles.GetListHashCode());

        public bool Equals(CompositeCollider other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = ColliderId.Equals(other.ColliderId)
                         && Triangles.ListEquals(other.Triangles);
            return result;
        }
    }
}