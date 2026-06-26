using System.Collections.Generic;
using System.Linq;
using System.Text;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Resources;
using BH.SDK.Models.Values;
using BH.SDK.Validations;

namespace BH.SDK.Utils
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
        public static Dictionary<TKey, TValue> CopyDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
            where TKey : unmanaged where TValue : ICopyable<TValue>
        {
            var copyDictionary = new Dictionary<TKey, TValue>(dictionary.Count);
            foreach (var (key, value) in dictionary)
                copyDictionary.Add(key, value.Copy());
            return copyDictionary;
        }
        
        public static bool ArrayEquals<T>(this T[] array, T[] other)
        {
            if (array is null || other is null) return false;
            if (ReferenceEquals(array, other)) return true;
            if (array.Length != other.Length) return false;
            var result = array.SequenceEqual(other);
            return result;
        }
        public static bool ListEquals<T>(this List<T> list, List<T> other)
        {
            if (list is null || other is null) return false;
            if (ReferenceEquals(list, other)) return true;
            if (list.Count != other.Count) return false;
            var result = list.SequenceEqual(other);
            return result;
        }
        public static bool DictionaryEquals<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> other)
        {
            if (dictionary is null || other is null) return false;
            if (ReferenceEquals(dictionary, other)) return true;
            if (dictionary.Count != other.Count) return false;
            
            foreach (var (key, value) in dictionary)
            {
                if (!other.TryGetValue(key, out var otherValue))
                    return false;
                if (!value.Equals(otherValue))
                    return false;
            }
            return true;
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
        public static int GetDictionaryHashCode<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary is null) return 0;
            unchecked
            {
                int hash = 17;
                foreach (var kvp in dictionary.OrderBy(kvp => kvp.Key))
                {
                    hash = hash * 31 + (kvp.Key?.GetHashCode() ?? 0);
                    hash = hash * 31 + (kvp.Value?.GetHashCode() ?? 0);
                }
                return hash;
            }
        }
        
        public static string GetPath(this List<RulePath> trace)
        {
            if (trace.Count == 0) return string.Empty;
            var builder = new StringBuilder();
            trace.BuildTracePath(builder);
            return builder.ToString();
        }
        public static void BuildTracePath(this List<RulePath> trace, StringBuilder builder)
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