namespace BHSDK.Models.Enum.Text
{
    // Copied from UniText, if source enum will change - mention it anywhere
    
    /// <summary>
    /// Specifies which metric defines the bottom edge of the text box.
    /// </summary>
    /// <remarks>
    /// Controls how the last line contributes to the total text height.
    /// Matches CSS <c>text-box-edge</c> under-edge values and Figma Vertical Trim.
    /// </remarks>
    public enum TextObjectUnderEdge : byte
    {
        /// <summary>Bottom edge at descent line — default, fits all descenders.</summary>
        Descent = 0,

        /// <summary>Bottom edge at baseline — tighter fit, matches Figma Vertical Trim.</summary>
        Baseline = 1,

        /// <summary>Bottom edge includes half-leading — matches CSS and Figma Standard mode.</summary>
        HalfLeading = 2,
    }
}