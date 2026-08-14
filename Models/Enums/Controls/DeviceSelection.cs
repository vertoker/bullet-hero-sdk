namespace BH.SDK.Models.Enums.Controls
{
    /// <summary>
    /// How the active control device is chosen.
    /// </summary>
    public enum DeviceSelection : byte
    {
        /// <summary>The game picks it: most recently used among the active, present, permitted ones,
        /// with the priority list breaking ties and deciding startup.</summary>
        Auto = 0,

        /// <summary>The player pinned one (ControlsSettings.Common.ManualDevice), and the game falls
        /// back to Auto only if that one cannot drive the current scene.</summary>
        Manual = 1,
    }
}
