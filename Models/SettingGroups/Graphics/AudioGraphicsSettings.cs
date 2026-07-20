using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.SettingGroups.Graphics
{
    public class AudioGraphicsSettings : BaseGraphicsSettings,
        IModel<AudioGraphicsSettings>, IMoveable<AudioGraphicsSettings>
    {
        [JsonProperty(Names.RenderEffects)]
        public bool RenderEffects { get; set; }
        
        [RuleMin(0f)]
        [JsonProperty(Names.MaxDiffTime)]
        public float MaxDiffTime { get; set; }
        
        [JsonProperty(Names.Scrub)]
        public bool UseScrub { get; set; }
        
        [RuleMin(0f)]
        [JsonProperty(Names.ScrubTime)]
        public float ScrubTime { get; set; }

        public AudioGraphicsSettings()
        {
            Render = true;
            RenderEffects = true;
            MaxDiffTime = 0.2f;
            UseScrub = true;
            ScrubTime = 0.1f;
        }
        public AudioGraphicsSettings(bool render, bool renderEffects,
            float maxDiffTime, bool useScrub, float scrubTime) : base(render)
        {
            RenderEffects = renderEffects;
            MaxDiffTime = maxDiffTime;
            UseScrub = useScrub;
            ScrubTime = scrubTime;
        }
        public override void Reset()
        {
            base.Reset();
            Render = true;
            RenderEffects = true;
            MaxDiffTime = 0.2f;
            UseScrub = true;
            ScrubTime = 0.1f;
        }
        
        public override object Clone() => CopyImpl();
        public override BaseGraphicsSettings Copy() => CopyImpl();
        AudioGraphicsSettings ICopyable<AudioGraphicsSettings>.Copy() => CopyImpl();
        
        private AudioGraphicsSettings CopyImpl() => new(Render, RenderEffects, MaxDiffTime, UseScrub, ScrubTime);
        
        public void Pull(AudioGraphicsSettings source)
        {
            Render = source.Render;
            RenderEffects = source.RenderEffects;
            MaxDiffTime = source.MaxDiffTime;
            UseScrub = source.UseScrub;
            ScrubTime = source.ScrubTime;
        }

        public override bool Equals(object obj) => obj is AudioGraphicsSettings value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            RenderEffects, MaxDiffTime, UseScrub, ScrubTime);

        public bool Equals(AudioGraphicsSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other)
                   && RenderEffects == other.RenderEffects
                   && MaxDiffTime.Equals(other.MaxDiffTime)
                   && UseScrub == other.UseScrub
                   && ScrubTime.Equals(other.ScrubTime);
        }
    }
}