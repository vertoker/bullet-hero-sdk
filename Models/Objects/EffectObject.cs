using System;
using System.Collections.Generic;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    [RuleContainer]
    public class EffectObject : RectObject, IModel<EffectObject>, IUpdatable<EffectObject>
    {
        public override ObjectType GetModelType() => ObjectType.EffectObject;

        [JsonProperty(Names.EffectId)]
        public EffectId EffectId { get; set; }
        
        public EffectObject()
        {
            EffectId = EffectId.Null;
        }
        public EffectObject(ObjectId objectId, ObjectId parentObjectId, string name, bool visible, int startFrame,
            int endFrame, int layer, List<PosKey> positions, List<AngleKey> rotations, List<ScaKey> scales, List<ScaKey> sizes,
            List<AlignmentKey> anchorsMin, List<AlignmentKey> anchorsMax, List<AlignmentKey> pivots, EffectId effectId)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, layer,
                positions, rotations, scales, sizes, anchorsMin, anchorsMax, pivots)
        {
            EffectId = effectId;
        }
        public override void Reset()
        {
            base.Reset();
            EffectId = EffectId.Null;
        }

        public override object Clone() => CopyImpl();
        public override RectObject Copy() => CopyImpl();
        EffectObject ICopyable<EffectObject>.Copy() => CopyImpl();
        
        private EffectObject CopyImpl() => new(ObjectId, ParentObjectId, Name, Visible, StartFrame, EndFrame, Layer,
            Positions.CopyList(), Rotations.CopyList(), Scales.CopyList(), Sizes.CopyList(),
            AnchorsMin.CopyList(), AnchorsMax.CopyList(), Pivots.CopyList(), EffectId);
        
        public void Update(EffectObject src)
        {
            base.Update(src);

            EffectId = src.EffectId;
        }

        public override bool Equals(object obj) => obj is EffectObject value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), EffectId);
        
        public bool Equals(EffectObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            var result = EqualsObject(other)
                         && EqualsEffectObject(other);
            return result;
        }
        public override bool Equals(RectObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            switch (other)
            {
                case EffectObject effectObject:
                {
                    var result = EqualsObject(effectObject)
                                 && EqualsEffectObject(effectObject);
                    return result;
                }
                default:
                {
                    var result = EqualsObject(other);
                    return result;
                }
            }
        }
        
        private bool EqualsEffectObject(EffectObject other)
        {
            var result = EffectId == other.EffectId;
            return result;
        }
    }
}