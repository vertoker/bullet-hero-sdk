using BHSDK.Models.Enum.Text;

namespace BHSDK.Models
{
    public static class TextStatic
    {
        public static readonly TextObjectDirection DirectionDefault = TextObjectDirection.Auto;
        public static readonly bool WordWrapDefault = true;
        public static readonly TextObjectHorizontalAlignment HorizontalAlignmentDefault = TextObjectHorizontalAlignment.Center;
        public static readonly TextObjectVerticalAlignment VerticalAlignmentDefault = TextObjectVerticalAlignment.Middle;
        public static readonly TextObjectOverEdge OverEdgeDefault = TextObjectOverEdge.Ascent;
        public static readonly TextObjectUnderEdge UnderEdgeDefault = TextObjectUnderEdge.Descent;
        public static readonly TextObjectLeadingDistribution LeadingDistributionDefault = TextObjectLeadingDistribution.HalfLeading;
    }
}