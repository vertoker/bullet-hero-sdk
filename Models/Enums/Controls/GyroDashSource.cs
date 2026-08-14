namespace BH.SDK.Models.Enums.Controls
{
    /// <summary>
    /// How dash is triggered while a motion sensor drives movement - the sensor itself has no
    /// buttons.
    /// </summary>
    public enum GyroDashSource : byte
    {
        /// <summary>A tap anywhere on the screen.</summary>
        AnyScreenTap = 0,

        /// <summary>One on-screen button, placed by its own anchor/size settings.</summary>
        ScreenButton = 1,
    }
}
