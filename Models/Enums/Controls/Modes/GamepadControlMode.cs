namespace BH.SDK.Models.Enums.Controls.Modes
{
    /// <summary>
    /// How a gamepad's sticks drive the avatar.
    /// </summary>
    public enum GamepadControlMode : byte
    {
        /// <summary>Stick deflection is the cursor's position inside the camera rect.</summary>
        Absolute = 0,

        /// <summary>Stick deflection moves the cursor.</summary>
        Relative = 1,

        /// <summary>The stick is the direction, no cursor - the gamepad default.</summary>
        Direction = 2,
    }
}
