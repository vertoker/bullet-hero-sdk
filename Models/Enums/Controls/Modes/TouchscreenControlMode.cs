namespace BH.SDK.Models.Enums.Controls.Modes
{
    /// <summary>
    /// How the touchscreen drives the avatar.
    /// </summary>
    public enum TouchscreenControlMode : byte
    {
        /// <summary>The finger itself is the cursor, offset so it does not cover the avatar.</summary>
        Absolute = 0,

        /// <summary>Dragging anywhere moves the cursor by the drag delta - the mobile default.</summary>
        Relative = 1,

        /// <summary>An on-screen joystick, no cursor.</summary>
        Direction = 2,
    }
}
