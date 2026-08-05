using BH.SDK.Models.Enum.Text;

// ReSharper disable InconsistentNaming

namespace BH.SDK.Rules
{
    public static class TextRules
    {
        public const float Size_X_Fallback = 1f;
        public const float Size_Y_Fallback = 1f;
        public const float FontSize_Fallback = 1f;

        // Font size is a keyframed float, so it otherwise inherits the generic +/-1e6 value range -
        // a range in which a negative or astronomically large size is legal data. Zero is allowed:
        // it means "invisible this frame", which is a normal way to animate text in and out.
        public const float MinFontSize = 0f;
        public const float MaxFontSize = 1000f;
        
        public const bool WordWrap_Default = true;
        public const TextObjectHorizontalAlignment HorizontalAlignment_Default = TextObjectHorizontalAlignment.Center;
        public const TextObjectVerticalAlignment VerticalAlignment_Default = TextObjectVerticalAlignment.Middle;
    }
}