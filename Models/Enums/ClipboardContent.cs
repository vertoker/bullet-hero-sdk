using System;

namespace BH.SDK.Models.Enums
{
    /// <summary> Which sections of a <see cref="Clipboard.ClipboardData"/> actually carry something. </summary>
    [Flags]
    public enum ClipboardContent : byte
    {
        None = 0,
        Objects = 1 << 0,
        PrefabObjects = 1 << 1,
        ObjectKeys = 1 << 2,
        AudioKeys = 1 << 3,
        AudioTracks = 1 << 4,
        EventKeys = 1 << 5,
    }
}
