namespace BH.SDK.Models.Enum.Keyframes
{
    public enum Color4X4KeyType : byte
    {
        Value = 0, // all colors setup with one color value
        Horizontal = 1, // setup by 2 colors: BL/TL for one and BR/TR for second
        Vertical = 2, // setup by 2 colors: BL/BR for one and TL/TR for second
        BariCentrical = 3, // all 4 colors separately, for each side
    }
}