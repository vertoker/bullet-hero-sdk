namespace BH.SDK.Models.Enums.Text
{
    // Which end of the string TextObject.Fillments writes from. The value itself is a keyframed
    // 0..1 float; this only decides which characters that fraction keeps, so all four are one
    // predicate over the character index rather than four code paths.

    /// <summary>
    /// The order a text object's characters are written in as its fill fraction rises.
    /// </summary>
    public enum TextFillDirection : byte
    {
        /// <summary>Writes from the first character onward - an ordinary typewriter.</summary>
        Forward = 0,

        /// <summary>Writes from the last character backward.</summary>
        Backward = 1,

        /// <summary>Grows outward from the middle in both directions at once.</summary>
        FromCenter = 2,

        /// <summary>Grows inward from both ends, meeting in the middle.</summary>
        ToCenter = 3,
    }
}
