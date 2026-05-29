using System;
using System.Collections.Generic;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Effects;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Models.Keyframes;
using BH.SDK.Rules.Attributes;
using BH.SDK.Utils;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Objects
{
    [RuleContainer]
    public class EffectObject : RectObject, IEffect,
        ICopyable<EffectObject>, IEquatable<EffectObject>, IUpdatable<EffectObject>
    {
        public override ObjectType GetModelType() => ObjectType.Effect;

        public static readonly Version Version = new(1, 0);
        public Version GetVersion() => Version;
        
        [RuleNotNull]
        [JsonProperty(Names.Core)]
        public EffectObjectCore Core { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Forces)]
        public EffectObjectForces Forces { get; set; }
        
        [RuleNotNull(typeof(EffectShapePoint))]
        [JsonProperty(Names.EffShape)]
        public IEffectShape EffectShape { get; set; }
        
        [RuleNotNull(typeof(EffectAngleValue))]
        [JsonProperty(Names.EffAngle)]
        public IEffectAngle EffectAngle { get; set; }
        
        [RuleNotNull(typeof(EffectScaleValue))]
        [JsonProperty(Names.EffScale)]
        public IEffectScale EffectScale { get; set; }
        
        [RuleNotNull(typeof(EffectColorValue))]
        [JsonProperty(Names.EffColor)]
        public IEffectColor EffectColor { get; set; }

        public EffectObject()
        {
            Core = new EffectObjectCore();
            Forces = new EffectObjectForces();
            EffectShape = new EffectShapePoint();
            EffectAngle = new EffectAngleValue();
            EffectScale = new EffectScaleValue();
            EffectColor = new EffectColorValue();
        }
        public EffectObject(int objectId, int parentObjectId, string name, bool visible, int startFrame, int endFrame,
            List<PosKey> positions, List<LayerKey> layers, List<AngleKey> rotations, List<ScaKey> scales, List<ScaKey> sizes,
            List<AlignmentKey> anchorsMin, List<AlignmentKey> anchorsMax, List<AlignmentKey> pivots,
            EffectObjectCore core, EffectObjectForces forces, IEffectShape effectShape,
            IEffectAngle effectAngle, IEffectScale effectScale, IEffectColor effectColor)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame,
                positions, layers, rotations, scales, sizes, anchorsMin, anchorsMax, pivots)
        {
            Core = core;
            Forces = forces;
            EffectShape = effectShape;
            EffectAngle = effectAngle;
            EffectScale = effectScale;
            EffectColor = effectColor;
        }

        public override object Clone() => CopyImpl();
        public override Object Copy() => CopyImpl();
        EffectObject ICopyable<EffectObject>.Copy() => CopyImpl();
        
        private EffectObject CopyImpl() => new(ObjectId, ParentObjectId, Name, Visible, StartFrame, EndFrame,
            Positions.CopyList(), Layers.CopyList(), Rotations.CopyList(), Scales.CopyList(), Sizes.CopyList(),
            AnchorsMin.CopyList(), AnchorsMax.CopyList(), Pivots.CopyList(),
            Core.Copy(), Forces.Copy(), EffectShape.Copy(), EffectAngle.Copy(), EffectScale.Copy(), EffectColor.Copy());
        
        public void Update(EffectObject src)
        {
            base.Update(this);
            
            Core.Update(src.Core);
            Forces.Update(src.Forces);
            EffectShape = src.EffectShape.Copy();
            EffectAngle = src.EffectAngle.Copy();
            EffectScale = src.EffectScale.Copy();
            EffectColor = src.EffectColor.Copy();
        }

        public override bool Equals(object obj) => obj is EffectObject value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            Core, Forces, EffectShape, EffectAngle, EffectScale, EffectColor);

        public bool Equals(EffectObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Core.Equals(other.Core)
                         && Forces.Equals(other.Forces)
                         && EffectShape.Equals(other.EffectShape)
                         && EffectAngle.Equals(other.EffectAngle)
                         && EffectScale.Equals(other.EffectScale)
                         && EffectColor.Equals(other.EffectColor);
            return result;
        }
    }
}