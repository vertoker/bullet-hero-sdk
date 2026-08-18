using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat.Models;
using BH.SDK.Models.Data;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;

namespace BH.SDK.Interop.AfterBeat.Import
{
    // One import writes into one scope - a level's own objects, or a prefab template's. Everything
    // that has to be shared across the whole run lives here rather than being threaded through
    // twenty parameters: the id table, the shape resources being synthesized on demand, the theme
    // literal colours are resolved against, and the report.
    //
    // The id table is the reason importing is TWO passes. Afterbeat identifies objects by string
    // and parents them by that string, in no particular order - a child can appear before its
    // parent. So every object is minted first and only then filled in, or half the parent links
    // resolve to nothing purely because of where they sat in the file.
    //
    // EffectiveLayers exists for the same kind of reason: Afterbeat's depth is ABSOLUTE and this
    // format's Layer is relative to the parent, so writing a child's layer needs its parent's
    // effective one, which is not stored anywhere on the model being built. It is filled by that
    // same first pass, and for the same reason - filling it as the second pass walks the list
    // answers zero for every parent that happens to be written later.

    /// <summary> Everything one Afterbeat import shares across its objects. </summary>
    public class ABImportContext
    {
        public ABOptions Options { get; }
        public InteropReport Report { get; }

        /// <summary> Where imported objects land. </summary>
        public IObjectScope Scope { get; }

        /// <summary> Where their ids come from. At level scope this is a DIFFERENT object from
        /// <see cref="Scope"/> - Level.Game is the scope, Level.Settings is the counter. </summary>
        public IObjectIdCounter Counter { get; }

        /// <summary> Level shape resources; synthesized shapes are added here on first use. May be
        /// null when a prefab is imported on its own, in which case nothing is synthesized. </summary>
        public IDictionary<ShapeId, CompositeShape> Shapes { get; }

        /// <summary> The theme a semi-transparent colour is resolved against. May be null. </summary>
        public ThemeData ReferenceTheme { get; set; }

        /// <summary> Afterbeat's own object id to the one minted for it here. </summary>
        public Dictionary<string, ObjectId> ObjectIds { get; } = new();

        /// <summary> Afterbeat's own object id to the effective (parent-chain-summed) layer it
        /// ended up with. </summary>
        public Dictionary<string, int> EffectiveLayers { get; } = new();

        /// <summary> The band of draw order this scope's own objects occupy, which is what the
        /// background is placed below and the prefab placements above. Both stay 0 until something
        /// is resolved, so a scope holding no objects puts its background on layer -1. </summary>
        public int LowestContentLayer { get; private set; }
        public int HighestContentLayer { get; private set; }

        public ABImportContext(ABOptions options, InteropReport report,
            IObjectScope scope, IObjectIdCounter counter,
            IDictionary<ShapeId, CompositeShape> shapes = null)
        {
            Options = options ?? new ABOptions();
            Report = report ?? new InteropReport();
            Scope = scope;
            Counter = counter;
            Shapes = shapes;
        }

        /// <summary> Mints an id for one source object, or hands back the one already minted for it.
        /// A source object with no id of its own gets an id nothing can reference, which is correct:
        /// nothing in the file can name it either. </summary>
        public ObjectId Mint(string sourceId)
        {
            if (!string.IsNullOrEmpty(sourceId) && ObjectIds.TryGetValue(sourceId, out var existing))
                return existing;

            var id = Counter.GetNextObjectId();
            if (!string.IsNullOrEmpty(sourceId)) ObjectIds[sourceId] = id;
            return id;
        }

        /// <summary> True while this context is filling a prefab TEMPLATE rather than a level. </summary>
        public bool IsPrefabScope => Scope is Prefab;

        /// <summary> Resolves a parent reference. "camera" is a real parent target here, exactly as
        /// it is there; an id nothing minted is a dangling reference and becomes a root. </summary>
        public ObjectId ResolveParent(string sourceParentId, string path)
        {
            if (string.IsNullOrEmpty(sourceParentId)) return ObjectId.Null;

            if (sourceParentId == VgdObject.CameraParentId)
            {
                // A template has no camera to hang off - it is content, placed later, possibly many
                // times. The source format lets a template object name one anyway; keeping it makes
                // a template this format's own rules reject (RuleParentObjectIdValid), on real
                // content, so it becomes a root of the template instead.
                if (!IsPrefabScope) return ObjectId.Camera;

                Report.Approximated("parent_camera_in_prefab",
                    "Objects inside an Afterbeat prefab can be parented to the camera; a template here has no camera, so those objects became roots of the template.",
                    path);
                return ObjectId.Null;
            }

            if (ObjectIds.TryGetValue(sourceParentId, out var parent)) return parent;

            Report.Approximated("parent_missing",
                "Some objects name a parent that is not in the level; those objects were imported as roots.",
                path);
            return ObjectId.Null;
        }

        /// <summary> Records what one source object's own depth means as an effective layer.
        /// Filled for the whole list before any object is read, so a parent is known however late
        /// in the document it was written. </summary>
        public void SetEffectiveLayer(string sourceId, int effectiveLayer)
        {
            if (string.IsNullOrEmpty(sourceId)) return;
            EffectiveLayers[sourceId] = effectiveLayer;
        }

        /// <summary> The effective layer of a source object's parent, or 0 for a root. </summary>
        public int GetParentEffectiveLayer(string sourceParentId)
        {
            if (string.IsNullOrEmpty(sourceParentId)) return 0;
            return EffectiveLayers.TryGetValue(sourceParentId, out var layer) ? layer : 0;
        }

        /// <summary> Widens the band this scope's content occupies. Called once per resolved object
        /// list rather than per object, so a scope built out of several lists still answers for all
        /// of them. </summary>
        public void RegisterContentLayers(int lowest, int highest)
        {
            if (lowest < LowestContentLayer) LowestContentLayer = lowest;
            if (highest > HighestContentLayer) HighestContentLayer = highest;
        }
    }
}
