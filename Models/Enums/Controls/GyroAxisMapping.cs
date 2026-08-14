namespace BH.SDK.Models.Enums.Controls
{
    /// <summary>
    /// Which two rotation axes of a motion sensor become the two screen axes.
    /// </summary>
    public enum GyroAxisMapping : byte
    {
        /// <summary>Turning left/right drives X, tilting forward/back drives Y - a pad held in two
        /// hands.</summary>
        YawPitch = 0,

        /// <summary>Rolling the device drives X, tilting forward/back drives Y - a phone held
        /// flat.</summary>
        RollPitch = 1,
    }
}
