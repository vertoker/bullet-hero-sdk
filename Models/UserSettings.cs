using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.SettingGroups;
using BH.SDK.Rules.Attributes;
using BH.SDK.Versions;
using Newtonsoft.Json;

namespace BH.SDK.Models
{
    // TODO Add tests for IResetable

    /// <summary>
    /// The player's own options, saved once per device (settings.json) - the third top-level file
    /// next to a level and its metadata. Nothing here travels with a level: the same level must
    /// play the same way regardless of these.
    /// </summary>
    [RuleContainer]
    [DataVersion(DataDomains.UserSettings, 1, 0)]
    public class UserSettings : IModel<UserSettings>, IMoveable<UserSettings>
    {
        /// <summary> Options fitting no other group (resource loading, later language). </summary>
        [RuleNotNull]
        [JsonProperty(Names.General)]
        public GeneralSettings General { get; set; }

        /// <summary> Input scheme and (eventually) key bindings. </summary>
        [RuleNotNull]
        [JsonProperty(Names.Controls)]
        public ControlsSettings Controls { get; set; }

        /// <summary> Volume mix. </summary>
        [RuleNotNull]
        [JsonProperty(Names.Audio)]
        public AudioSettings Audio { get; set; }

        /// <summary> Rendering quality and per-subsystem switches. </summary>
        [RuleNotNull]
        [JsonProperty(Names.Graphics)]
        public GraphicsSettings Graphics { get; set; }

        /// <summary> In-game level editor preferences. Present even for players who never open the
        /// editor - the file has a fixed shape. </summary>
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
        public void Reset()
        {
            General.Reset();
            Controls.Reset();
            Audio.Reset();
            Graphics.Reset();
            GameEditor.Reset();
        }

        public object Clone() => Copy();
        public UserSettings Copy() => new(General.Copy(), Controls.Copy(),
            Audio.Copy(), Graphics.Copy(), GameEditor.Copy());
        
        public void Pull(UserSettings source)
        {
            General.Pull(source.General);
            Controls.Pull(source.Controls);
            Audio.Pull(source.Audio);
            Graphics.Pull(source.Graphics);
            GameEditor.Pull(source.GameEditor);
        }

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