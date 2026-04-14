using System;
using System.Collections.Generic;
using BHSDK.Models.Components;
using BHSDK.Models.Effects;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using Object = BHSDK.Models.Base.Object;

namespace BHSDK.Models.Objects
{
    public class EffectObject : Object, IEffect
    {
        public override ObjectType GetModelType() => ObjectType.Effect;

        public Version GetVersion() => new(1, 0);
        
        [JsonProperty(ModelNames.Core)]
        public EffectObjectCore Core { get; set; }
        
        [JsonProperty(ModelNames.Force)]
        public EffectObjectForces Forces { get; set; }
        
        [JsonProperty(ModelNames.Effect + ModelNames.Shape)]
        public IEffectShape EffectShape { get; set; }
        
        [JsonProperty(ModelNames.Effect + ModelNames.Angle)]
        public IEffectAngle EffectAngle { get; set; }
        
        [JsonProperty(ModelNames.Effect + ModelNames.Scale)]
        public IEffectScale EffectScale { get; set; }
        
        [JsonProperty(ModelNames.Effect + ModelNames.Color)]
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
    }
}