using System;
using BHSDK.Models.Enum.Settings;
using BHSDK.Models.Interfaces;
using BHSDK.Models.SettingGroups.Graphics;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.SettingGroups
{
    [RuleContainer]
    public class GraphicsSettings : ICopyable<GraphicsSettings>, IEquatable<GraphicsSettings>
    {
        [JsonProperty(Names.FramerateTarget)]
        public FramerateTarget FramerateTarget { get; set; }
        
        // if 0 - doesn't setup framerate, use Unity default. Require project restart
        // if > 0 - target framerate
        
        [RuleMin(0)]
        [JsonProperty(Names.FixedFramerate)]
        public int FixedFramerate { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Audio)]
        public AudioGraphicsSettings Audio { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Effects)]
        public EffectsGraphicsSettings Effects { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Text)]
        public PostProcessingGraphicsSettings PostProcessing { get; set; }

        public GraphicsSettings()
        {
            FramerateTarget = FramerateTarget.ScreenHz;
            FixedFramerate = 60;
            Audio = new AudioGraphicsSettings();
            Effects = new EffectsGraphicsSettings();
            PostProcessing = new PostProcessingGraphicsSettings();
        }
        public GraphicsSettings(FramerateTarget framerateTarget, int fixedFramerate,
            AudioGraphicsSettings audio, EffectsGraphicsSettings effects,
            PostProcessingGraphicsSettings postProcessing)
        {
            FramerateTarget = framerateTarget;
            FixedFramerate = fixedFramerate;
            Audio = audio;
            Effects = effects;
            PostProcessing = postProcessing;
        }

        public object Clone() => Copy();
        public GraphicsSettings Copy() => new(FramerateTarget, FixedFramerate,
            Audio.Copy(), Effects.Copy(), PostProcessing.Copy());

        public override bool Equals(object obj) => obj is GraphicsSettings value && Equals(value);
        public override int GetHashCode() => HashCode.Combine((int)FramerateTarget,
            FixedFramerate, Audio, Effects, PostProcessing);

        public bool Equals(GraphicsSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return FramerateTarget == other.FramerateTarget
                   && FixedFramerate == other.FixedFramerate
                   && Audio.Equals(other.Audio)
                   && Effects.Equals(other.Effects)
                   && PostProcessing.Equals(other.PostProcessing);
        }
    }
}