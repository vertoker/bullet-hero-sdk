using System;

namespace BH.SDK.Models.Enum.Settings
{
    [Flags]
    public enum RenderStatus : byte
    {
        None = 0,
        Player = 1 << 0,
        Editor = 1 << 1,
        All = 255,
    }
}