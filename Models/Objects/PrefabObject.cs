using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    /// <summary>
    /// A placement of a Prefab template. Its children are not resolved at load time - they were
    /// materialized as ordinary objects in the hosting scope the moment PrefabId was set, and this
    /// object only keeps the bookkeeping needed to re-sync them when the template changes.
    /// </summary>
    [RuleContainer]
    public class PrefabObject : RectObject, IModel<PrefabObject>, IUpdatable<PrefabObject>
    {
        public override ObjectType GetModelType() => ObjectType.PrefabObject;

        /// <summary> Which template this placement instantiates. </summary>
        [RuleIPrimitiveGuidNotNull]
        [JsonProperty(Names.PrefabId)]
        public PrefabId PrefabId { get; set; } // reference into Level.Resources.Prefabs

        /// <summary> Remap table from template-inner ids to the real ids this placement's copies got
        /// in the hosting scope - how a resync finds the objects it already owns instead of
        /// duplicating them. </summary>
        [JsonProperty(Names.ObjectIds)]
        public Dictionary<ObjectId, ObjectId> ObjectIds { get; set; } // inner id -> this instance's outer id

        // Per-instance field overrides on this placement's own materialized children, keyed by
        // ModificationKey (TEMPLATE's inner ObjectId + field Path) - see
        // BH.Core.Services.PrefabMaterializer.ApplyModifications (re-applied after every
        // materialize/resync, on top of the fresh template copy) and GameEditor's
        // EditObjectOperation.RecordModification (what records one here whenever a direct edit lands
        // on a materialized child outside Prefab Mode). One Modification per (object, field) pair -
        // a child can have several fields overridden at once, but only one override per field.
        /// <summary> Per-placement field overrides, keyed by (template object, field path). </summary>
        [JsonProperty(Names.Mod)]
        public Dictionary<ModificationKey, Modification> Modifications { get; set; }

        public PrefabObject()
        {
            PrefabId = PrefabId.Null;
            ObjectIds = new Dictionary<ObjectId, ObjectId>();
            Modifications = new Dictionary<ModificationKey, Modification>();
        }
        public PrefabObject(ObjectId objectId, ObjectId parentObjectId, string name, bool visible, int startFrame, int endFrame, int layer,
            List<PosKey> positions, List<AngleKey> rotations, List<ScaKey> scales, List<ScaKey> sizes,
            List<AlignmentKey> anchorsMin, List<AlignmentKey> anchorsMax, List<AlignmentKey> pivots,
            PrefabId prefabId, Dictionary<ObjectId, ObjectId> objectIds, Dictionary<ModificationKey, Modification> modifications)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, layer,
                positions, rotations, scales, sizes, anchorsMin, anchorsMax, pivots)
        {
            PrefabId = prefabId;
            ObjectIds = objectIds;
            Modifications = modifications;
        }
        public override void Reset()
        {
            base.Reset();
            PrefabId = PrefabId.Null;
            ObjectIds.Clear();
            Modifications.Clear();
        }

        public override object Clone() => CopyImpl();
        public override RectObject Copy() => CopyImpl();
        PrefabObject ICopyable<PrefabObject>.Copy() => CopyImpl();

        private PrefabObject CopyImpl() => new(ObjectId, ParentObjectId, Name, Visible, StartFrame, EndFrame, Layer,
            Positions.CopyList(), Rotations.CopyList(), Scales.CopyList(), Sizes.CopyList(),
            AnchorsMin.CopyList(), AnchorsMax.CopyList(), Pivots.CopyList(),
            PrefabId, ObjectIds.CopyDictionaryUnmanaged(), Modifications.CopyDictionaryManaged());

        public void Update(PrefabObject src)
        {
            base.Update(src);

            PrefabId = src.PrefabId;
            ObjectIds = src.ObjectIds.CopyDictionaryUnmanaged();
            Modifications = src.Modifications.CopyDictionaryManaged();
        }

        public override bool Equals(object obj) => obj is PrefabObject value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(PrefabId);
            hashCode.Add(ObjectIds.GetDictionaryHashCode());
            hashCode.Add(Modifications.GetDictionaryHashCode());
            return hashCode.ToHashCode();
        }

        public bool Equals(PrefabObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            var result = EqualsObject(other)
                         && EqualsPrefabObject(other);
            return result;
        }
        public override bool Equals(RectObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            switch (other)
            {
                case PrefabObject prefabObject:
                {
                    var result = EqualsObject(prefabObject)
                                 && EqualsPrefabObject(prefabObject);
                    return result;
                }
                default:
                {
                    var result = EqualsObject(other);
                    return result;
                }
            }
        }

        private bool EqualsPrefabObject(PrefabObject other)
        {
            var result = PrefabId.Equals(other.PrefabId)
                         && ObjectIds.DictionaryEquals(other.ObjectIds)
                         && Modifications.DictionaryEquals(other.Modifications);
            return result;
        }
    }
}
