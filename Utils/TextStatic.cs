using BHSDK.Models.Enum.Text;
using UnityEngine;

namespace BHSDK.Utils
{
    public static class TextStatic
    {
        public static readonly Vector2 SizeFallback = new(1f, 1f);
        public static readonly float FontSizeFallback = 1f;
        
        public static readonly TextObjectDirection DirectionDefault = TextObjectDirection.Auto;
        public static readonly bool WordWrapDefault = true;
        public static readonly TextObjectHorizontalAlignment HorizontalAlignmentDefault = TextObjectHorizontalAlignment.Center;
        public static readonly TextObjectVerticalAlignment VerticalAlignmentDefault = TextObjectVerticalAlignment.Middle;
        public static readonly TextObjectOverEdge OverEdgeDefault = TextObjectOverEdge.Ascent;
        public static readonly TextObjectUnderEdge UnderEdgeDefault = TextObjectUnderEdge.Descent;
        public static readonly TextObjectLeadingDistribution LeadingDistributionDefault = TextObjectLeadingDistribution.HalfLeading;
    }
}