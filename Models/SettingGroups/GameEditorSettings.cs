using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.SettingGroups
{
    /// <summary>
    /// Preferences for the in-game level editor, per device: autosave policy and camera limits.
    /// Belongs to the person editing, never to the level being edited.
    /// </summary>
    [RuleContainer]
    [RulePropertyOrder(nameof(GameEditorSettings.CameraMinSize), nameof(GameEditorSettings.CameraMaxSize))]
    public class GameEditorSettings : IModel<GameEditorSettings>, IMoveable<GameEditorSettings>
    {
        // Savings

        /// <summary> Whether the editor saves on its own. </summary>
        [JsonProperty(Names.Autosave)]
        public bool Autosave { get; set; }

        /// <summary> Seconds between autosaves. </summary>
        [RuleMin(1f)]
        [JsonProperty(Names.AutosaveRate)]
        public float AutosaveRate { get; set; }

        /// <summary> How many autosaves are kept before the oldest is dropped - the depth of the
        /// safety net, traded against disk space. </summary>
        [RuleInRange(1, 1000)]
        [JsonProperty(Names.MaxAutosaveFiles)]
        public int MaxAutosaveFiles { get; set; }

        // Editor Camera

        /// <summary> Closest the editor camera may zoom in. </summary>
        [RuleMin(0f)]
        [JsonProperty(Names.CameraMinSize)]
        public float CameraMinSize { get; set; }

        /// <summary> Furthest the editor camera may zoom out. </summary>
        [RuleMin(0f)]
        [JsonProperty(Names.CameraMaxSize)]
        public float CameraMaxSize { get; set; }

        public GameEditorSettings()
        {
            Autosave = true;
            AutosaveRate = 60f;
            MaxAutosaveFiles = 25;
            CameraMinSize = 0.1f;
            CameraMaxSize = 100f;
        }
        public GameEditorSettings(bool autosave, float autosaveRate, int maxAutosaveFiles,
            float cameraMinSize, float cameraMaxSize)
        {
            Autosave = autosave;
            AutosaveRate = autosaveRate;
            MaxAutosaveFiles = maxAutosaveFiles;
            CameraMinSize = cameraMinSize;
            CameraMaxSize = cameraMaxSize;
        }
        public void Reset()
        {
            Autosave = true;
            AutosaveRate = 60f;
            MaxAutosaveFiles = 25;
            CameraMinSize = 0.1f;
            CameraMaxSize = 100f;
        }

        public object Clone() => Copy();
        public GameEditorSettings Copy() => new(Autosave, AutosaveRate, MaxAutosaveFiles, CameraMinSize, CameraMaxSize);

        public void Pull(GameEditorSettings source)
        {
            Autosave = source.Autosave;
            AutosaveRate = source.AutosaveRate;
            MaxAutosaveFiles = source.MaxAutosaveFiles;
            CameraMinSize = source.CameraMinSize;
            CameraMaxSize = source.CameraMaxSize;
        }

        public override bool Equals(object obj) => obj is GameEditorSettings value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Autosave, AutosaveRate, MaxAutosaveFiles, CameraMinSize, CameraMaxSize);

        public bool Equals(GameEditorSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Autosave == other.Autosave
                   && AutosaveRate.Equals(other.AutosaveRate)
                   && MaxAutosaveFiles == other.MaxAutosaveFiles
                   && CameraMinSize.Equals(other.CameraMinSize)
                   && CameraMaxSize.Equals(other.CameraMaxSize);
        }
    }
}