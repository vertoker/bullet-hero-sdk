using BHSDK.Models.Enum.Settings;
using BHSDK.Models.SettingGroups.Graphics;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.SettingGroups
{
    [RuleContainer]
    public class GraphicsSettings
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
    }
}