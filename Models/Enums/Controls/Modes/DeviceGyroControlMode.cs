namespace BH.SDK.Models.Enums.Controls.Modes
{
    /// <summary>
    /// How the device's own motion sensor drives the avatar.
    /// </summary>
    public enum DeviceGyroControlMode : byte
    {
        /// <summary>Tilt angle is the cursor's position.</summary>
        Absolute = 0,

        /// <summary>Angular velocity moves the cursor.</summary>
        Relative = 1,

        /// <summary>Tilt is the direction, no cursor - the default here, since a phone is held rather
        /// than aimed.</summary>
        Direction = 2,
    }
}
