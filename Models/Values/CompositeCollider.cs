using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Values
{
    [RuleContainer]
    public class CompositeCollider : ICopyable<CompositeCollider>, IEquatable<CompositeCollider>
    {
        public const int MaxCount = 64;
        
        // id for user-defined colliders, allowed only negative (started with -1, 0 is uninitialized)
        // this works like resource, but integrated in model. Works only with TextureObject
        
        [RuleMax(0)]
        [JsonProperty(Names.ColliderId)]
        public int ColliderId { get; set; }
        
        [RuleNotNull, RuleCollectionMaxCount(MaxCount)]
        [JsonProperty(Names.TrianglesShort)]
        public List<TriangleCollider> Triangles { get; set; }

        public CompositeCollider()
        {
            ColliderId = 0;
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