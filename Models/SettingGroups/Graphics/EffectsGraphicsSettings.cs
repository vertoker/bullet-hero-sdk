using System;
using BH.SDK.Models.Enum.Settings;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.SettingGroups.Graphics
{
    [RuleContainer]
    public class EffectsGraphicsSettings : BaseGraphicsSettings,
        ICopyable<EffectsGraphicsSettings>, IEquatable<EffectsGraphicsSettings>
    {
        [JsonProperty(Names.FramerateTarget)]
        public FramerateTarget FramerateTarget { get; set; }
        
        [RuleMin(0)]
        [JsonProperty(Names.FixedFramerate)]
        public int FixedFramerate { get; set; }
        
        [RuleMin(0.2f)]
        [JsonProperty(Names.MaxScrubTime)]
        public float MaxScrubTime { get; set; }

        public EffectsGraphicsSettings()
        {
            Render = true;
            FramerateTarget = FramerateTarget.Fixed;
            FixedFramerate = 50;
            MaxScrubTime = 0.5f;
        }
        public EffectsGraphicsSettings(bool render, FramerateTarget framerateTarget,
            int fixedFramerate, float maxScrubTime) : base(render)
        {
            FramerateTarget = framerateTarget;
            FixedFramerate = fixedFramerate;
            MaxScrubTime = maxScrubTime;
        }

        public object Clone() => Copy();
        public EffectsGraphicsSettings Copy() => new(Render, FramerateTarget, FixedFramerate, MaxScrubTime);

        public override bool Equals(object obj) => obj is EffectsGraphicsSettings value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            (int)FramerateTarget, FixedFramerate, MaxScrubTime);

        public bool Equals(EffectsGraphicsSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other)
                   && FramerateTarget == other.FramerateTarget
                   && FixedFramerate == other.FixedFramerate
                   && MaxScrubTime.Equals(other.MaxScrubTime);
        }
    }
}