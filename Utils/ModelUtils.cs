using System.Collections.Generic;
using System.Linq;
using System.Text;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Resources;
using BHSDK.Models.Values;
using BHSDK.Validations;

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
        
        public static bool ArrayEquals<T>(this T[] array, T[] other)
        {
            if (ReferenceEquals(array, other)) return true;
            if (array is null || other is null) return false;
            if (array.Length != other.Length) return false;
            var result = array.SequenceEqual(other);
            return result;
        }
        public static bool ListEquals<T>(this List<T> list, List<T> other)
        {
            if (ReferenceEquals(list, other)) return true;
            if (list is null || other is null) return false;
            if (list.Count != other.Count) return false;
            var result = list.SequenceEqual(other);
            return result;
        }
        
        public static int GetArrayHashCode<T>(this T[] array)
        {
            if (array is null) return 0;
            unchecked
            {
                int hash = 17;
                foreach (var item in array)
                    hash = hash * 31 + (item?.GetHashCode() ?? 0);
                return hash;
            }
        }
        public static int GetListHashCode<T>(this List<T> list)
        {
            if (list is null) return 0;
            unchecked
            {
                int hash = 17;
                foreach (var item in list)
                    hash = hash * 31 + (item?.GetHashCode() ?? 0);
                return hash;
            }
        }
        
        public static string GetPath(this List<LevelPath> trace)
        {
            if (trace.Count == 0) return string.Empty;
            var builder = new StringBuilder();
            trace.BuildTracePath(builder);
            return builder.ToString();
        }
        public static void BuildTracePath(this List<LevelPath> trace, StringBuilder builder)
        {
            if (trace.Count == 0) return;
            for (var i = 0; i < trace.Count - 1; i++)
            {
                var path = trace[i];
                path.Append(builder);
                builder.Append('.');
            }
            trace[^1].Append(builder);
        }
    }
}