using System;

namespace BH.SDK.Models.Enums.Controls
{
    // Named by POSITION, not by symbol (South rather than Cross/A), because the symbol is a brand question
    // and the position is not: a player switching pad families keeps the binding they set, and only a
    // future glyph layer would have to care which symbol is printed on that button.

    /// <summary>
    /// A set of gamepad buttons, used wherever more than one may be bound to the same action (dash,
    /// gyro activation, gyro recentre).
    /// </summary>
    [Flags]
    public enum GamepadButtonMask : ushort
    {
        None = 0,

        South = 1 << 0,
        East = 1 << 1,
        West = 1 << 2,
        North = 1 << 3,

        LeftShoulder = 1 << 4,
        RightShoulder = 1 << 5,
        LeftTrigger = 1 << 6,
        RightTrigger = 1 << 7,

        LeftStickPress = 1 << 8,
        RightStickPress = 1 << 9,

        Select = 1 << 10,
        Start = 1 << 11,
    }
}
