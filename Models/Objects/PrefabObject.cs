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
    [RuleContainer]
    public class PrefabObject : RectObject, IModel<PrefabObject>, IUpdatable<PrefabObject>
    {
        public override ObjectType GetModelType() => ObjectType.PrefabObject;

        [RuleIPrimitiveGuidNotNull]
        [JsonProperty(Names.PrefabId)]
        public PrefabId PrefabId { get; set; } // reference into Level.Resources.Prefabs

        [JsonProperty(Names.ObjectIds)]
        public Dictionary<ObjectId, ObjectId> ObjectIds { get; set; } // inner id -> this instance's outer id

        public PrefabObject()
        {
            PrefabId = PrefabId.Null;
            ObjectIds = new Dictionary<ObjectId, ObjectId>();
        }
        public PrefabObject(ObjectId objectId, ObjectId parentObjectId, string name, bool visible, int startFrame, int endFrame, int layer,
            List<PosKey> positions, List<AngleKey> rotations, List<ScaKey> scales, List<ScaKey> sizes,
            List<AlignmentKey> anchorsMin, List<AlignmentKey> anchorsMax, List<AlignmentKey> pivots,
            PrefabId prefabId, Dictionary<ObjectId, ObjectId> objectIds)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, layer,
                positions, rotations, scales, sizes, anchorsMin, anchorsMax, pivots)
        {
            PrefabId = prefabId;
            ObjectIds = objectIds;
        }
        public override void Reset()
        {
            base.Reset();
            PrefabId = PrefabId.Null;
            ObjectIds.Clear();
        }

        public override object Clone() => CopyImpl();
        public override RectObject Copy() => CopyImpl();
        PrefabObject ICopyable<PrefabObject>.Copy() => CopyImpl();

        private PrefabObject CopyImpl() => new(ObjectId, ParentObjectId, Name, Visible, StartFrame, EndFrame, Layer,
            Positions.CopyList(), Rotations.CopyList(), Scales.CopyList(), Sizes.CopyList(),
            AnchorsMin.CopyList(), AnchorsMax.CopyList(), Pivots.CopyList(),
            PrefabId, ObjectIds.CopyDictionaryUnmanaged());

        public void Update(PrefabObject src)
        {
            base.Update(src);

            PrefabId = src.PrefabId;
            ObjectIds = src.ObjectIds.CopyDictionaryUnmanaged();
        }

        public override bool Equals(object obj) => obj is PrefabObject value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(PrefabId);
            hashCode.Add(ObjectIds.GetDictionaryHashCode());
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
                         && ObjectIds.DictionaryEquals(other.ObjectIds);
            return result;
        }
    }
}
