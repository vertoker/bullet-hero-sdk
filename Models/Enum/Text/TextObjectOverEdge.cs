namespace BHSDK.Models.Enum.Text
{
    // Copied from UniText, if source enum will change - mention it anywhere
    
    /// <summary>
    /// Specifies which metric defines the top edge of the text box.
    /// </summary>
    /// <remarks>
    /// Controls how the first line is positioned relative to the container top.
    /// Matches CSS <c>text-box-edge</c> over-edge values and Figma Vertical Trim.
    /// </remarks>
    public enum TextObjectOverEdge : byte
    {
        /// <summary>Top edge at ascent line — default, fits all ascenders and diacritics.</summary>
        Ascent = 0,

        /// <summary>Top edge at cap height — tighter fit, matches Figma Vertical Trim.</summary>
        CapHeight = 1,

        /// <summary>Top edge includes half-leading — matches CSS and Figma Standard mode.</summary>
        HalfLeading = 2,
    }
}