using System;
using System.Collections.Generic;
using BH.SDK.Models.Effects;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Effects;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Models.Keyframes;
using BH.SDK.Rules;
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
        
        [JsonProperty(Names.HasStopLocalFrame)]
        public bool HasStopLocalFrame { get; set; }
        
        [RuleLevelFrame]
        [JsonProperty(Names.StopLocalFrame)]
        public int StopLocalFrame { get; set; }
        
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
            HasStopLocalFrame = EffectRules.HasStopLocalFrame_Default;
            StopLocalFrame = EffectRules.StopLocalFrame_Default;
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
            bool hasStopLocalFrame, int stopLocalFrame, EffectObjectCore core, EffectObjectForces forces,
            IEffectShape effectShape, IEffectAngle effectAngle, IEffectScale effectScale, IEffectColor effectColor)
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame,
                positions, layers, rotations, scales, sizes, anchorsMin, anchorsMax, pivots)
        {
            HasStopLocalFrame = hasStopLocalFrame;
            StopLocalFrame = stopLocalFrame;
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
            HasStopLocalFrame, StopLocalFrame, Core.Copy(), Forces.Copy(),
            EffectShape.Copy(), EffectAngle.Copy(), EffectScale.Copy(), EffectColor.Copy());
        
        public void Update(EffectObject src)
        {
            base.Update(this);
            
            HasStopLocalFrame = src.HasStopLocalFrame;
            StopLocalFrame = src.StopLocalFrame;
            Core.Update(src.Core);
            Forces.Update(src.Forces);
            EffectShape = src.EffectShape.Copy();
            EffectAngle = src.EffectAngle.Copy();
            EffectScale = src.EffectScale.Copy();
            EffectColor = src.EffectColor.Copy();
        }

        public override bool Equals(object obj) => obj is EffectObject value && Equals(value);
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(HasStopLocalFrame);
            hashCode.Add(StopLocalFrame);
            hashCode.Add(Core);
            hashCode.Add(Forces);
            hashCode.Add(EffectShape);
            hashCode.Add(EffectAngle);
            hashCode.Add(EffectScale);
            hashCode.Add(EffectColor);
            return hashCode.ToHashCode();
        }
        
        public bool Equals(EffectObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            
            var result = EqualsObject(other)
                         && EqualsRectObject(other)
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
                                 && EqualsRectObject(effectObject)
                                 && EqualsEffectObject(effectObject);
                    return result;
                }
                default:
                {
                    var result = EqualsObject(other)
                                 && EqualsRectObject(other);
                    return result;
                }
            }
        }
        public override bool Equals(Object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            switch (other)
            {
                case EffectObject effectObject:
                {
                    var result = EqualsObject(effectObject)
                                 && EqualsRectObject(effectObject)
                                 && EqualsEffectObject(effectObject);
                    return result;
                }
                case RectObject rectObject:
                {
                    var result = EqualsObject(rectObject)
                                 && EqualsRectObject(rectObject);
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
            var result = HasStopLocalFrame == other.HasStopLocalFrame
                         && StopLocalFrame.Equals(other.StopLocalFrame)
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