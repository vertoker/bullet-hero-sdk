using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Utils;
using Newtonsoft.Json;

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
            Loop = EffectStatic.Core_Loop_Default;
            ParticleCount = EffectStatic.Core_ParticleCount_Default;
            LifetimeBounds = new Vector2Value(
                EffectStatic.Core_LifetimeBounds_X_Default,
                EffectStatic.Core_LifetimeBounds_Y_Default);
            HasStopLocalFrame = EffectStatic.Core_HasStopLocalFrame_Default;
            StopLocalFrame = EffectStatic.Core_StopLocalFrame_Default;
            TextureResourceId = EffectStatic.Core_TextureResourceId_Default;
            ParticlePivot = new Alignment(new Vector2Value(
                EffectStatic.Core_Pivot_X_Default,
                EffectStatic.Core_Pivot_Y_Default));
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