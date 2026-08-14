using System;
using BH.SDK.Services.Controls;

namespace BH.SDK.Models.Enums.Controls.Modes
{
    /// <summary>
    /// Which <see cref="ControlMode"/>s a device supports at all - static device information, never
    /// persisted. See <see cref="ControlDeviceInfo"/>.
    /// </summary>
    [Flags]
    public enum ControlModeMask : byte
    {
        None = 0,
        Absolute = 1 << 0,
        Relative = 1 << 1,
        Direction = 1 << 2,

        All = Absolute | Relative | Direction,
    }
}
