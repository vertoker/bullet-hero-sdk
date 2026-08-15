using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.SettingGroups
{
    /// <summary>
    /// Preferences for the in-game level editor, per device: autosave policy, camera limits and how
    /// the preview player starts. Belongs to the person editing, never to the level being edited.
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

        // Preview player

        // The preview player's toggle is also what decides who owns the viewport's touches, so its
        // starting state is a real preference rather than a constant: a desktop author wants the player
        // there from the first frame (a mouse loses nothing to it), a phone author does not, since the
        // whole viewport goes to the avatar the moment it exists. The platform only picks the value a
        // FRESH settings file is born with - after that it is the author's own.

        /// <summary> Whether the editor's preview player starts switched on. </summary>
        [JsonProperty(Names.PlayerActiveDefault)]
        public bool PlayerActiveDefault { get; set; }

        /// <summary> Whether switching the preview player on drops the gizmo mode back to None. </summary>
        [JsonProperty(Names.GizmosResetOnPlayer)]
        public bool GizmosResetOnPlayer { get; set; }

        // Selection

        // Whether multi-selection is a MODIFIER or a MODE is a real preference, not a constant: on a
        // desktop the modifier is the familiar answer and an ordinary click should still replace the
        // selection, while on touch there is no key to hold, so the mode has to apply on its own.
        // Turning this off makes every click additive for as long as the mode is on.
        //
        // A mode entered by LONG PRESS ignores this either way, and has to: a finger has no modifier
        // to hold, so gating it there would make the touch entry point reach a mode that never applies.

        /// <summary> Whether multi-selection applies only while the modifier is held down. </summary>
        [JsonProperty(Names.MultiSelectRequiresHold)]
        public bool MultiSelectRequiresHold { get; set; }

        public GameEditorSettings()
        {
            ResetOwn();
        }
        public GameEditorSettings(bool autosave, float autosaveRate, int maxAutosaveFiles,
            float cameraMinSize, float cameraMaxSize, bool playerActiveDefault, bool gizmosResetOnPlayer,
            bool multiSelectRequiresHold)
        {
            Autosave = autosave;
            AutosaveRate = autosaveRate;
            MaxAutosaveFiles = maxAutosaveFiles;
            CameraMinSize = cameraMinSize;
            CameraMaxSize = cameraMaxSize;
            PlayerActiveDefault = playerActiveDefault;
            GizmosResetOnPlayer = gizmosResetOnPlayer;
            MultiSelectRequiresHold = multiSelectRequiresHold;
        }
        public void Reset()
        {
            ResetOwn();
        }
        private void ResetOwn()
        {
            Autosave = true;
            AutosaveRate = 60f;
            MaxAutosaveFiles = 25;
            CameraMinSize = 0.1f;
            CameraMaxSize = 100f;
            PlayerActiveDefault = true;
            GizmosResetOnPlayer = true;
            MultiSelectRequiresHold = true;
        }

        public object Clone() => Copy();
        public GameEditorSettings Copy() => new(Autosave, AutosaveRate, MaxAutosaveFiles, CameraMinSize,
            CameraMaxSize, PlayerActiveDefault, GizmosResetOnPlayer, MultiSelectRequiresHold);

        public void Pull(GameEditorSettings source)
        {
            Autosave = source.Autosave;
            AutosaveRate = source.AutosaveRate;
            MaxAutosaveFiles = source.MaxAutosaveFiles;
            CameraMinSize = source.CameraMinSize;
            CameraMaxSize = source.CameraMaxSize;
            PlayerActiveDefault = source.PlayerActiveDefault;
            GizmosResetOnPlayer = source.GizmosResetOnPlayer;
            MultiSelectRequiresHold = source.MultiSelectRequiresHold;
        }

        public override bool Equals(object obj) => obj is GameEditorSettings value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Autosave, AutosaveRate, MaxAutosaveFiles,
            CameraMinSize, CameraMaxSize, PlayerActiveDefault, GizmosResetOnPlayer, MultiSelectRequiresHold);

        public bool Equals(GameEditorSettings other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Autosave == other.Autosave
                   && AutosaveRate.Equals(other.AutosaveRate)
                   && MaxAutosaveFiles == other.MaxAutosaveFiles
                   && CameraMinSize.Equals(other.CameraMinSize)
                   && CameraMaxSize.Equals(other.CameraMaxSize)
                   && PlayerActiveDefault == other.PlayerActiveDefault
                   && GizmosResetOnPlayer == other.GizmosResetOnPlayer
                   && MultiSelectRequiresHold == other.MultiSelectRequiresHold;
        }
    }
}