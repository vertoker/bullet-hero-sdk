using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    [RuleContainer]
    public class EffectObjectCore : ICopyable<EffectObjectCore>, IUpdatable<EffectObjectCore>
    {
        [JsonProperty(Names.Loop)]
        public bool Loop { get; set; }
        
        [RuleInRange(EffectRules.Core.ParticleCount_Min, EffectRules.Core.ParticleCount_Max)]
        [JsonProperty(Names.ParticleCount)]
        public uint ParticleCount { get; set; }
        
        [RuleNotNull, RuleIVector2Min(EffectRules.Core.LifetimeBounds_Min)]
        [JsonProperty(Names.Lifetime)]
        public IVector2 LifetimeBounds { get; set; }
        
        [JsonProperty(Names.HasStopLocalFrame)]
        public bool HasStopLocalFrame { get; set; }
        
        [RuleLevelFrame]
        [JsonProperty(Names.StopLocalFrame)]
        public int StopLocalFrame { get; set; }
        
        // Same logic as TextureObject.TextureId
        
        [JsonProperty(Names.TextureResourceId)]
        public int TextureResourceId { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.ParticlePivot)]
        public Alignment ParticlePivot { get; set; }

        public EffectObjectCore()
        {
            Loop = EffectRules.Core.Loop_Default;
            ParticleCount = EffectRules.Core.ParticleCount_Default;
            LifetimeBounds = new Vector2Value(
                EffectRules.Core.LifetimeBounds_X_Default,
                EffectRules.Core.LifetimeBounds_Y_Default);
            HasStopLocalFrame = EffectRules.HasStopLocalFrame_Default;
            StopLocalFrame = EffectRules.StopLocalFrame_Default;
            TextureResourceId = EffectRules.Core.TextureResourceId_Default;
            ParticlePivot = new Alignment(new Vector2Value(
                EffectRules.Core.Pivot_X_Default,
                EffectRules.Core.Pivot_Y_Default));
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

        public object Clone() => Copy();
        public EffectObjectCore Copy() => new(Loop, ParticleCount, LifetimeBounds.Copy(),
            HasStopLocalFrame, StopLocalFrame, TextureResourceId, ParticlePivot.Copy());
    }
}