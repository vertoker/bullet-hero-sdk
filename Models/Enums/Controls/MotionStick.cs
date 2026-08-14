namespace BH.SDK.Models.Enums.Controls
{
    // Renumbered when Both was added, deliberately and with the old values NOT kept: a settings file is
    // local device state, and one player-visible dropdown reading one step off once is a smaller price
    // than a permanent hole at 0 in an enum every rule validates against.

    /// <summary>
    /// Which gamepad stick drives movement. Both reads whichever of the two is pushed further.
    /// </summary>
    public enum MotionStick : byte
    {
        Both = 0,
        Left = 1,
        Right = 2,
    }
}
