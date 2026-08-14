using System;

namespace BH.SDK.Models.Enums
{
    /// <summary> Which edges of a FrameSpan follow the edges of their parent's span. </summary>
    [Flags]
    public enum FrameAnchor : byte
    {
        None = 0,
        Start = 1 << 0,
        End = 1 << 1,
        Both = Start | End,
    }
}
