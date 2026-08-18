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
            if (parentId == ObjectId.Camera || parentId == CameraScaleRootId)
                return Models.VgdObject.CameraParentId;
            if (parentId == ObjectId.Null) return string.Empty;
            if (Scope?.Objects != null && Scope.Objects.ContainsKey(parentId)) return ToSourceId(parentId);

            Report.Approximated("parent_unresolvable",
                "Some objects are parented to something Afterbeat has no way to name (the local player, or a prefab root); they export as roots.",
                null);
            return string.Empty;
        }

        // The mirror of ABImportContext.CameraScaleRootId. An import rebuilds the source game's
        // camera-scale node as an ordinary object because this format's camera carries no scale;
        // writing that node back out would be wrong twice over - the node is not content the author
        // made, and the source game applies the very same factor itself, so its content would be
        // scaled by the zoom squared. So the node is dropped and its children are written as
        // parented to the camera directly, which is exactly where they came from.
        //
        // Identified by all three of: parented to the camera, a plain RectObject (nothing drawn),
        // and carrying the name the import gave it. An author who builds an empty object under the
        // camera and names it that gets it flattened too - the cost is its own scale track, and the
        // alternative is a format field for "this object is bookkeeping", which is a bigger thing
        // than this deserves.

        /// <summary> The rebuilt camera-scale node of this scope, or Null when there is none. </summary>
        public ObjectId CameraScaleRootId => _cameraScaleRootId ??= FindCameraScaleRoot();

        private ObjectId? _cameraScaleRootId;

        private ObjectId FindCameraScaleRoot()
        {
            if (Scope?.Objects == null) return ObjectId.Null;

            foreach (var pair in Scope.Objects)
            {
                var target = pair.Value;
                if (target == null || target.GetType() != typeof(RectObject)) continue;
                if (target.ParentObjectId != ObjectId.Camera) continue;
                if (target.Name != Import.ABLevelImporter.CameraScaleRootName) continue;

                return pair.Key;
            }

            return ObjectId.Null;
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
