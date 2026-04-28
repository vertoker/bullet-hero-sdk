using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectObjectCore : IUpdatable<EffectObjectCore>
    {
        [JsonProperty(Names.Loop)]
        public bool Loop { get; set; }
        
        [JsonProperty(Names.ParticleCount)]
        public uint ParticleCount { get; set; }
        
        [JsonProperty(Names.Lifetime)]
        public IVector2 LifetimeBounds { get; set; }
        
        [JsonProperty(Names.HasStopLocalFrame)]
        public bool HasStopLocalFrame { get; set; }
        
        [JsonProperty(Names.StopLocalFrame)]
        public int StopLocalFrame { get; set; }
        
        // Same logic as TextureObject.TextureId
        
        [JsonProperty(Names.TextureResourceId)]
        public int TextureResourceId { get; set; }
        
        [JsonProperty(Names.ParticlePivot)]
        public Alignment ParticlePivot { get; set; }

        public EffectObjectCore()
        {
            Loop = EffectStatic.Core_LoopDefault;
            ParticleCount = EffectStatic.Core_ParticleCountDefault;
            LifetimeBounds = new Vector2Value(EffectStatic.Core_LifetimeBoundsDefault);
            HasStopLocalFrame = EffectStatic.Core_HasStopLocalFrameDefault;
            StopLocalFrame = EffectStatic.Core_StopLocalFrameDefault;
            TextureResourceId = EffectStatic.Core_TextureResourceIdDefault;
            ParticlePivot = new Alignment(new Vector2Value(EffectStatic.Core_PivotDefault));
        }
        
        public EffectObjectCore(bool loop, uint particleCount, Vector2 lifetimeBounds,
            bool hasStopLocalFrame, int stopLocalFrame, int textureResourceId, Vector2 particlePivot)
        {
            Loop = loop;
            ParticleCount = particleCount;
            LifetimeBounds = new Vector2Value(lifetimeBounds);
            HasStopLocalFrame = hasStopLocalFrame;
            StopLocalFrame = stopLocalFrame;
            TextureResourceId = textureResourceId;
            ParticlePivot = new Alignment(new Vector2Value(particlePivot));
        }
        
        public EffectObjectCore(bool loop, uint particleCount, IVector2 lifetimeBounds,
            bool hasStopLocalFrame, int stopLocalFrame, int textureResourceId, Alignment particlePivot)
        {
            Loop = loop;
            ParticleCount = particleCount;
            LifetimeBounds = lifetimeBounds;
            HasStopLocalFrame = hasStopLocalFrame;
            StopLocalFrame = stopLocalFrame;
            TextureResourceId = textureResourceId;
            ParticlePivot = particlePivot;
        }

        public void Update(EffectObjectCore src)
        {
            Loop = src.Loop;
            ParticleCount = src.ParticleCount;
            LifetimeBounds = src.LifetimeBounds;
            HasStopLocalFrame = src.HasStopLocalFrame;
            StopLocalFrame = src.StopLocalFrame;
            TextureResourceId = src.TextureResourceId;
            ParticlePivot = src.ParticlePivot;
        }
    }
}