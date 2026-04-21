namespace BHSDK.Models.Enum.Text
{
    // Copied from UniText, if source enum will change - mention it anywhere
    
    /// <summary>
    /// Controls how extra leading from line-height is distributed relative to the content area.
    /// </summary>
    /// <remarks>
    /// Different platforms use different models:
    /// <list type="bullet">
    /// <item><see cref="HalfLeading"/> — CSS standard: split equally above and below.</item>
    /// <item><see cref="LeadingAbove"/> — Figma / iOS: all extra space above the line.</item>
    /// <item><see cref="LeadingBelow"/> — Android View / legacy: all extra space below the line.</item>
    /// </list>
    /// </remarks>
    public enum TextObjectLeadingDistribution : byte
    {
        /// <summary>Extra leading split equally above and below (CSS half-leading model).</summary>
        HalfLeading = 0,

        /// <summary>All extra leading placed above the line (Figma model).</summary>
        LeadingAbove = 1,

        /// <summary>All extra leading placed below the line (Android View model).</summary>
        LeadingBelow = 2,
    }
}