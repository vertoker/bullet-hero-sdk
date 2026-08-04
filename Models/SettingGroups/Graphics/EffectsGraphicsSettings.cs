using System;
using BH.SDK.Models.Enum.Settings;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.SettingGroups.Graphics
{
    /// <summary>
    /// Particle rendering options with their own framerate budget - effects can be simulated slower
    /// than the game itself, which is the main lever for keeping heavy levels playable on phones.
    /// </summary>
    [RuleContainer]
    public class EffectsGraphicsSettings : BaseGraphicsSettings, IFrameable,
        IModel<EffectsGraphicsSettings>, IMoveable<EffectsGraphicsSettings>
    {
        /// <summary> Where the effect update rate comes from - separate from the game's own, hence
        /// the duplicate of GraphicsSettings' pair of fields. </summary>
        [JsonProperty(Names.FramerateTarget)]
        public FramerateTarget FramerateTarget { get; set; }

        /// <summary> Explicit effect update rate, used when FramerateTarget says so. Lower than the
        /// game framerate by default. </summary>
        [RuleMin(1)]
        [JsonProperty(Names.FixedFramerate)]
        public int FixedFramerate { get; set; }

        /// <summary> Longest effect state the player will fast-forward when seeking, before it gives
        /// up and starts the effect fresh. </summary>
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
        public override void Reset()
        {
            base.Reset();
            Render = true;
            FramerateTarget = FramerateTarget.Fixed;
            FixedFramerate = 50;
            MaxScrubTime = 0.5f;
        }
        
        public override object Clone() => CopyImpl();
        public override BaseGraphicsSettings Copy() => CopyImpl();
        EffectsGraphicsSettings ICopyable<EffectsGraphicsSettings>.Copy() => CopyImpl();
        
        private EffectsGraphicsSettings CopyImpl() => new(Render, FramerateTarget, FixedFramerate, MaxScrubTime);
        
        public void Pull(EffectsGraphicsSettings source)
        {
            Render = source.Render;
            FramerateTarget = source.FramerateTarget;
            FixedFramerate = source.FixedFramerate;
            MaxScrubTime = source.MaxScrubTime;
        }

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