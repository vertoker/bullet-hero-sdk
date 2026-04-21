namespace BHSDK.Models.Enum.Text
{
    // Copied from UniText, if source enum will change - mention it anywhere
    
    /// <summary>
    /// Specifies the base text direction for bidirectional text processing. Copied from UniText
    /// </summary>
    public enum TextObjectDirection : byte
    {
        /// <summary>Left-to-right direction (e.g., Latin, Cyrillic).</summary>
        LeftToRight = 0,

        /// <summary>Right-to-left direction (e.g., Arabic, Hebrew).</summary>
        RightToLeft = 1,

        /// <summary>Automatically detect direction from text content using UAX #9.</summary>
        Auto = 2
    }
}