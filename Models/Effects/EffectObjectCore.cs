using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Primitives.Resources;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Effects
{
    [RuleContainer]
    public class EffectObjectCore : ICopyable<EffectObjectCore>, IEquatable<EffectObjectCore>, IUpdatable<EffectObjectCore>
    {
        [JsonProperty(Names.Render)]
        public bool Render { get; set; }
        
        [JsonProperty(Names.Local)]
        public bool Loop { get; set; }
        
        // For user-space it's always Local
        
        [RuleInRange(EffectRules.Core.ParticleCount_Min, EffectRules.Core.ParticleCount_Max)]
        [JsonProperty(Names.ParticleCount)]
        public uint ParticleCount { get; set; }
        
        [RuleNotNull, RuleIVector2Min(EffectRules.Core.LifetimeBounds_Min)]
        [JsonProperty(Names.Lifetime)]
        public IVector2 LifetimeBounds { get; set; }
        
        // Same logic as TextureObject.TextureId
        
        [JsonProperty(Names.TextureResourceId)]
        public TextureResourceId TextureResourceId { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.ParticlePivot)]
        public Alignment ParticlePivot { get; set; }

        public EffectObjectCore()
        {
            Render = EffectRules.Core.Render_Default;
            Loop = EffectRules.Core.Loop_Default;
            ParticleCount = EffectRules.Core.ParticleCount_Default;
            LifetimeBounds = new Vector2Value(
                EffectRules.Core.LifetimeBounds_X_Default,
                EffectRules.Core.LifetimeBounds_Y_Default);
            TextureResourceId = EffectRules.Core.TextureResourceId_Default;
            ParticlePivot = new Alignment(new Vector2Value(
                EffectRules.Core.Pivot_X_Default,
                EffectRules.Core.Pivot_Y_Default));
        }
        public EffectObjectCore(bool render, bool loop, uint particleCount,
            IVector2 lifetimeBounds, TextureResourceId textureResourceId, Alignment particlePivot)
        {
            Render = render;
            Loop = loop;
            ParticleCount = particleCount;
            LifetimeBounds = lifetimeBounds;
            TextureResourceId = textureResourceId;
            ParticlePivot = particlePivot;
        }

        public void Update(EffectObjectCore src)
        {
            Render = src.Render;
            Loop = src.Loop;
            ParticleCount = src.ParticleCount;
            LifetimeBounds = src.LifetimeBounds;
            TextureResourceId = src.TextureResourceId;
            ParticlePivot = src.ParticlePivot;
        }

        public object Clone() => Copy();
        public EffectObjectCore Copy() => new(Render, Loop, ParticleCount,
            LifetimeBounds.Copy(), TextureResourceId, ParticlePivot.Copy());

        public override bool Equals(object obj) => obj is EffectObjectCore value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Render, Loop, ParticleCount,
            LifetimeBounds, TextureResourceId, ParticlePivot);

        public bool Equals(EffectObjectCore other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Render == other.Render
                         && Loop == other.Loop
                         && ParticleCount.Equals(other.ParticleCount)
                         && LifetimeBounds.Equals(other.LifetimeBounds)
                         && TextureResourceId.Equals(other.TextureResourceId)
                         && ParticlePivot.Equals(other.ParticlePivot);
            return result;
        }
    }
}