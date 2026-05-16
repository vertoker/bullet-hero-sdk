using System;
using BHSDK.Models.Interfaces.SaveData;
using BHSDK.Models.SettingGroups;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models
{
    [RuleContainer]
    public class UserSettings : IUserSettings
    {
        public Version GetVersion() => new(1, 0);
        
        [RuleNotNull]
        [JsonProperty(Names.General)]
        public GeneralSettings General { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Controls)]
        public ControlsSettings Controls { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Audio)]
        public AudioSettings Audio { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Graphics)]
        public GraphicsSettings Graphics { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.GameEditor)]
        public GameEditorSettings GameEditor { get; set; }

        public UserSettings()
        {
            General = new GeneralSettings();
            Controls = new ControlsSettings();
            Audio = new AudioSettings();
            Graphics = new GraphicsSettings();
            GameEditor = new GameEditorSettings();
        }
        public UserSettings(GeneralSettings general, ControlsSettings controls,
            AudioSettings audio, GraphicsSettings graphics, GameEditorSettings gameEditor)
        {
            General = general;
            Controls = controls;
            Audio = audio;
            Graphics = graphics;
            GameEditor = gameEditor;
        }
    }
}