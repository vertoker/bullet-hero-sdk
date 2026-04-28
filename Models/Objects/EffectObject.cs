using System;
using System.Collections.Generic;
using BHSDK.Models.Effects;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Objects
{
    public class EffectObject : Object, IEffect, IUpdatable<EffectObject>
    {
        public override ObjectType GetModelType() => ObjectType.Effect;

        public Version GetVersion() => new(1, 0);
        
        [JsonProperty(Names.Core)]
        public EffectObjectCore Core { get; set; }
        
        [JsonProperty(Names.Forces)]
        public EffectObjectForces Forces { get; set; }
        
        [JsonProperty(Names.EffShape)]
        public IEffectShape EffectShape { get; set; }
        
        [JsonProperty(Names.EffAngle)]
        public IEffectAngle EffectAngle { get; set; }
        
        [JsonProperty(Names.EffScale)]
        public IEffectScale EffectScale { get; set; }
        
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
            List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Alignment pivot, EffectObjectCore core,
            EffectObjectForces forces, IEffectShape effectShape, IEffectAngle effectAngle, IEffectScale effectScale, IEffectColor effectColor) 
            : base(objectId, parentObjectId, name, visible, startFrame, endFrame, pos, rot, sca, layer, pivot)
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