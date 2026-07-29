using System;
using System.Collections.Generic;

namespace BH.SDK.Utils
{
    public static class TypeExtensions
    {
        public static bool IsList(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
        public static Type GetListGenericParameterOrDefault(this Type type)
        {
            return type.IsList() ? type.GetGenericArguments()[0] : null;
        }
        public static bool IsDictionary(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }
        public static Type GetDictionaryValueGenericParameterOrDefault(this Type type)
        {
            return type.IsDictionary() ? type.GetGenericArguments()[1] : null;
        }
    }
}