namespace BH.SDK.Models.Enums.Controls
{
    /// <summary>
    /// Which mouse button holds the "follow the cursor" state in Absolute mode.
    /// </summary>
    public enum MouseButton : byte
    {
        Left = 0,
        Right = 1,
        Middle = 2,

        /// <summary>Any of the three counts as held.</summary>
        Any = 3,
    }
}
