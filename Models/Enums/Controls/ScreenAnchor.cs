namespace BH.SDK.Models.Enums.Controls
{
    /// <summary>
    /// Where an on-screen control sits. Only the nine corner/edge/centre positions, since a thumb
    /// control is placed by reach rather than by coordinate; Handedness mirrors the whole layout on
    /// top of this.
    /// </summary>
    public enum ScreenAnchor : byte
    {
        BottomLeft = 0,
        BottomCenter = 1,
        BottomRight = 2,
        CenterLeft = 3,
        Center = 4,
        CenterRight = 5,
        TopLeft = 6,
        TopCenter = 7,
        TopRight = 8,
    }
}
