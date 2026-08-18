using System.Collections.Generic;
using BH.SDK.Models.Data;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules;

namespace BH.SDK.Interop.AfterBeat.Export
{
    // The mirror of ABImportContext. Two things it has to do that the import does not:
    //
    // Ids go the other way. This format numbers objects and Afterbeat names them with strings, so
    // an id is stringified - and it must be stringified the SAME way everywhere, or a parent
    // reference and the object it names stop matching.
    //
    // Layers go the other way too, and this is the direction that actually needs a walk: Layer here
    // is parent-relative, depth there is absolute, so exporting one object means summing its whole
    // parent chain. Doing that per object is quadratic on a deep hierarchy, so the answer is cached
    // as it is computed.

    /// <summary> Everything one Afterbeat export shares across its objects. </summary>
    public class ABExportContext
    {
        public ABOptions Options { get; }
        public InteropReport Report { get; }
        public IObjectScope Scope { get; }
        public ThemeData ReferenceTheme { get; set; }

        private readonly Dictionary<ObjectId, int> _effectiveLayers = new();

        public ABExportContext(ABOptions options, InteropReport report, IObjectScope scope)
        {
            Options = options ?? new ABOptions();
            Report = report ?? new InteropReport();
            Scope = scope;
        }

        /// <summary> This format's object id as the string Afterbeat names objects with. </summary>
        public static string ToSourceId(ObjectId id) => id.value.ToString();

        /// <summary> A parent reference. The camera is a parent on both sides; anything else
        /// unresolvable becomes a root, which is what Afterbeat does with a dangling one anyway. </summary>
        public string ToParentId(ObjectId parentId)
        {
            if (parentId == ObjectId.Camera) return Models.VgdObject.CameraParentId;
            if (parentId == ObjectId.Null) return string.Empty;
            if (Scope?.Objects != null && Scope.Objects.ContainsKey(parentId)) return ToSourceId(parentId);

            Report.Approximated("parent_unresolvable",
                "Some objects are parented to something Afterbeat has no way to name (the local player, or a prefab root); they export as roots.",
                null);
            return string.Empty;
        }

        /// <summary> An object's effective layer - its own plus every ancestor's, which is what this
        /// format means by draw order. Bounded by the format's own depth cap, so a cycle in
        /// hand-edited data cannot hang the export. </summary>
        public int GetEffectiveLayer(RectObject target)
        {
            if (target == null) return 0;
            if (_effectiveLayers.TryGetValue(target.ObjectId, out var cached)) return cached;

            var layer = target.Layer;
            var current = target;
            var depth = 0;

            while (depth++ < LevelRules.MaxObjectDepth)
            {
                var parentId = current.ParentObjectId;
                if (parentId == ObjectId.Null || Scope?.Objects == null) break;
                if (!Scope.Objects.TryGetValue(parentId, out var parent) || parent == null) break;

                if (_effectiveLayers.TryGetValue(parentId, out var parentLayer))
                {
                    layer += parentLayer;
                    break;
                }

                layer += parent.Layer;
                current = parent;
            }

            _effectiveLayers[target.ObjectId] = layer;
            return layer;
        }
    }
}
