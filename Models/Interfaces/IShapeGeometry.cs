using System.Collections.Generic;
using BH.SDK.Models.Values;

namespace BH.SDK.Models.Interfaces
{
    /// <summary>
    /// Indexed triangle geometry: shared corners plus the triples that build triangles out of them.
    /// The contract RuleShapeGeometry validates against, so the rule reads a typed pair rather than
    /// two properties found by name.
    /// </summary>
    public interface IShapeGeometry
    {
        /// <summary> Corners, each used by one or more triangles. </summary>
        List<Vector2Value> Vertices { get; }

        /// <summary> Triangle corners as indices into <see cref="Vertices"/>, three per triangle. </summary>
        List<int> Indices { get; }
    }
}
