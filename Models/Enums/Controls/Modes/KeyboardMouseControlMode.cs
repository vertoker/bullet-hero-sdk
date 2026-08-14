namespace BH.SDK.Models.Enums.Controls.Modes
{
    // One mode enum per device, and the duplication is the point: an unsupported mode must be
    // unrepresentable, not merely invalid, so a device that later drops a mode or gains a unique one
    // changes here alone. Values line up with ControlMode at 0/1/2 so the map to it stays trivial and
    // RuleEnumValid still validates against this device's own set.

    /// <summary>
    /// How keyboard and mouse drive the avatar.
    /// </summary>
    public enum KeyboardMouseControlMode : byte
    {
        /// <summary>The mouse position is the cursor - the PC default.</summary>
        Absolute = 0,

        /// <summary>The OS cursor is locked and its delta moves the in-world cursor.</summary>
        Relative = 1,

        /// <summary>WASD, no cursor.</summary>
        Direction = 2,
    }
}
