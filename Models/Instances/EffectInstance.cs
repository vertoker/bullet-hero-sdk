using System.Collections.Generic;
using BHSDK.Models.Base;
using BHSDK.Models.Components;
using BHSDK.Models.Effects;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Instances
{
    public class EffectInstance : Instance
    {
        public override InstanceType GetModelType() => InstanceType.Effect;

        // Core
        
        [JsonProperty(ModelNames.Loop)]
        public bool Loop { get; set; }
        
        [JsonProperty(ModelNames.Particle + ModelNames.Count)]
        public int ParticleCount { get; set; }
        
        [JsonProperty(ModelNames.Lifetime)]
        public IVector2 LifetimeBounds { get; set; }
        
        [JsonProperty(ModelNames.Particle + ModelNames.Collider)]
        public bool ParticleCollider { get; set; }
        
        [JsonProperty(ModelNames.HasStopTime)]
        public bool HasStopTime { get; set; }
        
        [JsonProperty(ModelNames.StopTime)]
        public float StopTime { get; set; }
        
        [JsonProperty(ModelNames.Particle + ModelNames.Texture + ModelNames.Index)]
        public int ParticleTextureIndex { get; set; }
        
        [JsonProperty(ModelNames.Particle + ModelNames.Pivot)]
        public IVector2 ParticlePivot { get; set; }
        
        // Types
        
        [JsonProperty(ModelNames.Force)]
        public EffectInstanceForces Forces { get; set; }
        
        [JsonProperty(ModelNames.Effect + ModelNames.Shape)]
        public IEffectShape EffectShape { get; set; }
        
        [JsonProperty(ModelNames.Effect + ModelNames.Angle)]
        public IEffectAngle EffectAngle { get; set; }
        
        [JsonProperty(ModelNames.Effect + ModelNames.Scale)]
        public IEffectScale EffectScale { get; set; }
        
        [JsonProperty(ModelNames.Effect + ModelNames.Color)]
        public IEffectColor EffectColor { get; set; }

        public EffectInstance()
        {
            Loop = true;
            ParticleCount = 10;
            LifetimeBounds = new Vector2Value(3f, 3f);
            ParticleCollider = false;
            HasStopTime = false;
            StopTime = 10f;
            ParticleTextureIndex = 0;
            ParticlePivot = new Vector2Value(0f, 0f);
            
            Forces = new EffectInstanceForces();
            EffectShape = new EffectShapePoint();
            EffectAngle = new EffectAngleValue();
            EffectScale = new EffectScaleValue();
            EffectColor = new EffectColorValue();
        }

        public EffectInstance(int instanceId, int parentInstanceId, string name, bool isVisible, 
            int startFrame, int endFrame, List<Pos> pos, List<Rot> rot, List<Sca> sca, int layer, Anchor pivot, 
            bool loop, int particleCount, IVector2 lifetimeBounds, bool particleCollider, bool hasStopTime, 
            float stopTime, int particleTextureIndex, IVector2 particlePivot, EffectInstanceForces forces, 
            IEffectShape effectShape, IEffectAngle effectAngle, IEffectScale effectScale, IEffectColor effectColor)
            : base(instanceId, parentInstanceId, name, isVisible, startFrame, endFrame, pos, rot, sca, layer, pivot)
        {
            Loop = loop;
            ParticleCount = particleCount;
            LifetimeBounds = lifetimeBounds;
            ParticleCollider = particleCollider;
            HasStopTime = hasStopTime;
            StopTime = stopTime;
            ParticleTextureIndex = particleTextureIndex;
            ParticlePivot = particlePivot;
            Forces = forces;
            EffectShape = effectShape;
            EffectAngle = effectAngle;
            EffectScale = effectScale;
            EffectColor = effectColor;
        }
    }
}