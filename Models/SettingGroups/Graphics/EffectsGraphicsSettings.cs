using BHSDK.Models.Enum.Settings;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.SettingGroups.Graphics
{
    [RuleContainer]
    public class EffectsGraphicsSettings : BaseGraphicsSettings
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
    }
}