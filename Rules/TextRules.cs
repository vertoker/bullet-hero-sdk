using BH.SDK.Models.Enum.Text;

// ReSharper disable InconsistentNaming

namespace BH.SDK.Rules
{
    public static class TextRules
    {
        public const float Size_X_Fallback = 1f;
        public const float Size_Y_Fallback = 1f;
        public const float FontSize_Fallback = 1f;
        
        public const TextObjectDirection Direction_Default = TextObjectDirection.Auto;
        public const bool WordWrap_Default = true;
        public const TextObjectHorizontalAlignment HorizontalAlignment_Default = TextObjectHorizontalAlignment.Center;
        public const TextObjectVerticalAlignment VerticalAlignment_Default = TextObjectVerticalAlignment.Middle;
        public const TextObjectOverEdge OverEdge_Default = TextObjectOverEdge.Ascent;
        public const TextObjectUnderEdge UnderEdge_Default = TextObjectUnderEdge.Descent;
        public const TextObjectLeadingDistribution LeadingDistribution_Default = TextObjectLeadingDistribution.HalfLeading;
    }
}