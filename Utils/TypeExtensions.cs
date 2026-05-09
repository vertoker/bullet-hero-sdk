using System;
using System.Collections.Generic;

namespace BHSDK.Utils
{
    public static class TypeExtensions
    {
        public static bool IsList(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        public static Type GetListGenericParameter(this Type type)
        {
            return type.GetGenericArguments()[0];
        }
        public static Type GetListGenericParameterOrDefault(this Type type)
        {
            return type.IsList() ? type.GetListGenericParameter() : null;
        }
    }
}