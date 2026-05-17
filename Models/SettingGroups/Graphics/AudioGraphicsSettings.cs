using System;
using BHSDK.Models.Interfaces;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.SettingGroups.Graphics
{
    public class AudioGraphicsSettings : BaseGraphicsSettings,
        ICopyable<AudioGraphicsSettings>, IEquatable<AudioGraphicsSettings>
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

        public object Clone() => Copy();
        public AudioGraphicsSettings Copy() => new(Render, RenderEffects, MaxDiffTime, UseScrub, ScrubTime);

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