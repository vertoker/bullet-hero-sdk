namespace BH.SDK.Models.Enums.Meta
{
    // The numeric value IS the minimum age, not an ordinal, so the number can be shown directly
    // ("12+") and two ratings compare as plain bytes.
    //
    // Unrated is 0, the LOWEST value, so an unfilled or pre-existing record never silently claims to
    // be adult content. It means "not declared", and a consumer that needs to be careful must check
    // for it explicitly rather than read it as "safe for everyone".
    //
    // Deliberately one scale rather than separate ESRB/PEGI/RARS fields: user-generated levels are
    // not submitted to any rating board, so a per-board value would be a guess three times over. A
    // minimum age maps onto all three well enough for the one thing it is for - telling a player what
    // they are about to launch.

    /// <summary> Minimum age a piece of content is authored for. </summary>
    public enum AgeRating : byte
    {
        /// <summary> Nothing was declared. Not a claim that the content is safe. </summary>
        Unrated = 0,

        /// <summary> Suitable for everyone (PEGI 3 / ESRB Everyone). </summary>
        Everyone = 3,
        Age6 = 6,
        Age12 = 12,
        Age16 = 16,
        Age18 = 18,
    }
}
