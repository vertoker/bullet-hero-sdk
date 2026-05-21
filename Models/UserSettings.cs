using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.SaveData;
using BH.SDK.Models.SettingGroups;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models
{
    [RuleContainer]
    public class UserSettings : IUserSettings, ICopyable<UserSettings>, IEquatable<UserSettings>
    {
        public static readonly Version Version = new(1, 0);
        public Version GetVersion() => Version;
        
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

        public object Clone() => Copy();
        public UserSettings Copy() => new(General.Copy(), Controls.Copy(),
            Audio.Copy(), Graphics.Copy(), GameEditor.Copy());

        public bool Equals(UserSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return General.Equals(other.General)
                   && Controls.Equals(other.Controls)
                   && Audio.Equals(other.Audio)
                   && Graphics.Equals(other.Graphics)
                   && GameEditor.Equals(other.GameEditor);
        }

        public override bool Equals(object obj) => obj is UserSettings value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(General, Controls, Audio, Graphics, GameEditor);
    }
}