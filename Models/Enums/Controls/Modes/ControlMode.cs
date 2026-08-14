namespace BH.SDK.Models.Enums.Controls.Modes
{
    /// <summary>
    /// How a device's input becomes avatar movement: an absolute cursor position, a cursor delta, or
    /// a bare direction with no cursor at all.
    /// </summary>
    public enum ControlMode : byte
    {
        /// <summary>Input names a position; the cursor jumps there and the avatar chases it.</summary>
        Absolute = 0,

        /// <summary>Input names a movement; the cursor accumulates it and the avatar chases it.</summary>
        Relative = 1,

        /// <summary>Input names a direction; there is no cursor and dash reads the avatar's own
        /// angle.</summary>
        Direction = 2,
    }
}
