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
        
        [JsonProperty(ModelNames.HasStopTime)]
        public bool HasStopTime { get; set; }
        
        [JsonProperty(ModelNames.StopTime)]
        public float StopTime { get; set; }
        
        // Same logic as TextureObject.TextureId
        
        [JsonProperty(ModelNames.Particle + ModelNames.Texture + ModelNames.Id)]
        public int ParticleTextureId { get; set; }
        
        [JsonProperty(ModelNames.Particle + ModelNames.Pivot)]
        public IVector2 ParticlePivot { get; set; }

        public EffectObjectCore()
        {
            Loop = EffectStatic.LoopDefault;
            ParticleCount = EffectStatic.ParticleCountDefault;
            LifetimeBounds = new Vector2Value(EffectStatic.LifetimeBoundsDefault);
            ParticleCollider = EffectStatic.ParticleColliderDefault;
            HasStopTime = EffectStatic.HasStopTimeDefault;
            StopTime = EffectStatic.StopTimeDefault;
            ParticleTextureId = EffectStatic.ParticleTextureIdDefault;
            ParticlePivot = new Vector2Value(EffectStatic.PivotDefault);
        }
        
        public EffectObjectCore(bool loop, uint particleCount, Vector2 lifetimeBounds, bool particleCollider,
            bool hasStopTime, float stopTime, int particleTextureId, Vector2 particlePivot)
        {
            Loop = loop;
            ParticleCount = particleCount;
            LifetimeBounds = new Vector2Value(lifetimeBounds);
            ParticleCollider = particleCollider;
            HasStopTime = hasStopTime;
            StopTime = stopTime;
            ParticleTextureId = particleTextureId;
            ParticlePivot = new Vector2Value(particlePivot);
        }
        
        public EffectObjectCore(bool loop, uint particleCount, IVector2 lifetimeBounds, bool particleCollider,
            bool hasStopTime, float stopTime, int particleTextureId, IVector2 particlePivot)
        {
            Loop = loop;
            ParticleCount = particleCount;
            LifetimeBounds = lifetimeBounds;
            ParticleCollider = particleCollider;
            HasStopTime = hasStopTime;
            StopTime = stopTime;
            ParticleTextureId = particleTextureId;
            ParticlePivot = particlePivot;
        }
    }
}