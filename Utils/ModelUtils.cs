using System.Collections.Generic;
using System.Linq;
using BHSDK.Models.Interfaces;
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

        public static T[] CopyArray<T>(this T[] array) where T : ICopyable<T>
        {
            var copyArray = new T[array.Length];
            array.CopyTo(copyArray, 0);
            return copyArray;
        }
        public static List<T> CopyList<T>(this List<T> list) where T : ICopyable<T>
        {
            var copyList = new List<T>(list.Count);
            foreach (var item in list)
                copyList.Add(item.Copy());
            return copyList;
        }
    }
}