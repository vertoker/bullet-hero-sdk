using System;
using System.Collections.Generic;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Primitives;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using BH.SDK.Versions;
using Newtonsoft.Json;

namespace BH.SDK.Models.Data
{
    /// <summary>
    /// A reusable collision shape built out of triangles, referenced by TextureObject.ColliderId.
    /// Shared rather than embedded, so hundreds of identical bullets cost one shape definition.
    /// </summary>
    [RuleContainer]
    [DataVersion(DataDomains.CompositeCollider, 1, 0)]
    public class CompositeCollider : IModel<CompositeCollider>
    {
        /// <summary> Identity of this shape - either a built-in id or a level-defined one. </summary>
        [RuleIPrimitiveGuidNotNull]
        [JsonProperty(Names.ColliderId)]
        public ColliderId ColliderId { get; set; }

        /// <summary> Editor-facing label of the shape. </summary>
        [JsonProperty(Names.ColliderName)] public string ColliderName { get; set; }

        // TODO (MAYBE) add Pivot for collider and maybe add it into collision process
        // TODO also most reason for it - extend game editor, because game colliders has it only for visuals
        // TODO or this can be PreferredPivot, it can be used by user in optional pivot in selection

        /// <summary> The shape itself, as a triangle soup in the object's local rect space. Concave
        /// shapes are expressed by using several triangles, never by winding order. </summary>
        [RuleNotNull, RuleCollectionMaxCount(ValueRules.MaxColliderTriangles)]
        [JsonProperty(Names.TrianglesShort)]
        public List<TriangleCollider> Triangles { get; set; }

        public CompositeCollider()
        {
            ColliderId = ColliderId.Null;
            ColliderName = string.Empty;
            Triangles = new List<TriangleCollider>();
        }

        public CompositeCollider(ColliderId colliderId, string colliderName, List<TriangleCollider> triangles)
        {
            ColliderId = colliderId;
            ColliderName = colliderName;
            Triangles = triangles;
        }

        public void Reset()
        {
            ColliderId = ColliderId.Null;
            ColliderName = string.Empty;
            Triangles.Clear();
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