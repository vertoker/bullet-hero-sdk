using BHSDK.Models.Resources;
using BHSDK.Models.Values;

namespace BHSDK.Utils
{
    public static class ModelUtils
    {
        private const float ByteMaxValue = byte.MaxValue;
        
        public static ColorValue ToColorValue(this Pixel pixel) => new(
            pixel.r / ByteMaxValue, 
            pixel.g / ByteMaxValue, 
            pixel.b / ByteMaxValue, 
            pixel.a / ByteMaxValue);
        
        public static Pixel ToPixel(this ColorValue colorValue) => new(
            (byte)(colorValue.R * ByteMaxValue),
            (byte)(colorValue.G * ByteMaxValue),
            (byte)(colorValue.B * ByteMaxValue),
            (byte)(colorValue.A * ByteMaxValue));
    }
}