using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectObjectCore
    {
        [JsonProperty(ModelNames.Loop)]
        public bool Loop { get; set; }
        
        [JsonProperty(ModelNames.Particle + ModelNames.Count)]
        public uint ParticleCount { get; set; }
        
        [JsonProperty(ModelNames.Lifetime)]
        public IVector2 LifetimeBounds { get; set; }
        
        [JsonProperty(ModelNames.Particle + ModelNames.Collider)]
        public bool ParticleCollider { get; set; }
        
        [JsonProperty(ModelNames.Has + ModelNames.Stop + ModelNames.Local + ModelNames.Frame)]
        public bool HasStopLocalFrame { get; set; }
        
        [JsonProperty(ModelNames.Stop + ModelNames.Local + ModelNames.Frame)]
        public int StopLocalFrame { get; set; }
        
        // Same logic as TextureObject.TextureId
        
        [JsonProperty(ModelNames.Particle + ModelNames.Texture + ModelNames.Id)]
        public int ParticleTextureId { get; set; }
        
        [JsonProperty(ModelNames.Particle + ModelNames.Pivot)]
        public IVector2 ParticlePivot { get; set; }

        public EffectObjectCore()
        {
            Loop = EffectStatic.Core_LoopDefault;
            ParticleCount = EffectStatic.Core_ParticleCountDefault;
            LifetimeBounds = new Vector2Value(EffectStatic.Core_LifetimeBoundsDefault);
            ParticleCollider = EffectStatic.Core_ParticleColliderDefault;
            HasStopLocalFrame = EffectStatic.Core_HasStopLocalFrameDefault;
            StopLocalFrame = EffectStatic.Core_StopLocalFrameDefault;
            ParticleTextureId = EffectStatic.Core_ParticleTextureIdDefault;
            ParticlePivot = new Vector2Value(EffectStatic.Core_PivotDefault);
        }
        
        public EffectObjectCore(bool loop, uint particleCount, Vector2 lifetimeBounds, bool particleCollider,
            bool hasStopLocalFrame, int stopLocalFrame, int particleTextureId, Vector2 particlePivot)
        {
            Loop = loop;
            ParticleCount = particleCount;
            LifetimeBounds = new Vector2Value(lifetimeBounds);
            ParticleCollider = particleCollider;
            HasStopLocalFrame = hasStopLocalFrame;
            StopLocalFrame = stopLocalFrame;
            ParticleTextureId = particleTextureId;
            ParticlePivot = new Vector2Value(particlePivot);
        }
        
        public EffectObjectCore(bool loop, uint particleCount, IVector2 lifetimeBounds, bool particleCollider,
            bool hasStopLocalFrame, int stopLocalFrame, int particleTextureId, IVector2 particlePivot)
        {
            Loop = loop;
            ParticleCount = particleCount;
            LifetimeBounds = lifetimeBounds;
            ParticleCollider = particleCollider;
            HasStopLocalFrame = hasStopLocalFrame;
            StopLocalFrame = stopLocalFrame;
            ParticleTextureId = particleTextureId;
            ParticlePivot = particlePivot;
        }
    }
}