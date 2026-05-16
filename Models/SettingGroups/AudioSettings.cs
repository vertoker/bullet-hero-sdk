using BHSDK.Models.Values;
using BHSDK.Rules.Attributes;

namespace BHSDK.Models.SettingGroups
{
    [RuleContainer]
    public class AudioSettings
    {
        [RuleInRange(0f, 1f)]
        public float Volume { get; set; }
        
        [RuleInRange(0f, 1f)]
        public float Game { get; set; }
        
        [RuleInRange(0f, 1f)]
        public float UI { get; set; }

        public AudioSettings()
        {
            Volume = 1f;
            Game = 1f;
            UI = 1f;
        }
        public AudioSettings(float volume, float game, float ui)
        {
            Volume = volume;
            Game = game;
            UI = ui;
        }
    }
}