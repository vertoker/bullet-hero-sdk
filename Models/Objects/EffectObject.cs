using System;
using System.Collections.Generic;
using BHSDK.Models.Effects;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    [RuleContainer]
    public class EffectObject : Object, IEffect, IUpdatable<EffectObject>
    {
        public override ObjectType GetModelType() => ObjectType.Effect;

        public Version GetVersion() => new(1, 0);
        
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
            List<Pos> positions, List<Rot> rotations, List<Sca> scales, int layer, Alignment pivot, EffectObjectCore core,
            EffectObjectForces forces, IEffectShape effectShape, IEffectAngle effectAngle, IEffectScale effectScale, IEffectColor effectColor) 
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, positions, rotations, scales, layer, pivot)
        {
            Core = core;
            Forces = forces;
            EffectShape = effectShape;
            EffectAngle = effectAngle;
            EffectScale = effectScale;
            EffectColor = effectColor;
        }

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
    }
}