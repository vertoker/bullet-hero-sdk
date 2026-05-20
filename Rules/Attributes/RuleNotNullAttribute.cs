using System;
using System.Linq;
using System.Reflection;
using BHSDK.Utils;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleNotNullAttribute : BaseRuleAttribute
    {
        public Type DefaultConstructType { get; set; }
        public object[] DefaultConstructArgs { get; set; }
        public Type[] DefaultConstructArgTypes { get; set; }

        public RuleNotNullAttribute()
        {
            
        }
        public RuleNotNullAttribute(Type defaultConstructType)
        {
            DefaultConstructType = defaultConstructType;
        }
        public RuleNotNullAttribute(params object[] defaultConstructArgs)
        {
            DefaultConstructArgs = defaultConstructArgs;
            DefaultConstructArgTypes = defaultConstructArgs
                .Select(a => a?.GetType() ?? typeof(object)).ToArray();
        }
        public RuleNotNullAttribute(Type defaultConstructType, params object[] defaultConstructArgs)
        {
            DefaultConstructType = defaultConstructType;
            DefaultConstructArgs = defaultConstructArgs;
            DefaultConstructArgTypes = defaultConstructArgs
                .Select(a => a?.GetType() ?? typeof(object)).ToArray();
        }

        protected override bool IsValidInternal(object value, object context)
        {
            return value != null;
        }
        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            var valueType = DefaultConstructType ?? property.PropertyType;
            var value = CreateDefaultValue(valueType);
            if (value != null) property.SetValue(target, value);
        }
        
        private object CreateDefaultValue(Type type)
        {
            // struct
            if (type.IsValueType) return Activator.CreateInstance(type);
            
            // "Special" types
            if (type == typeof(string)) return string.Empty;
            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                return Array.CreateInstance(elementType, 0);
            }
            if (type.IsList())
            {
                return Activator.CreateInstance(type);
            }
            
            // Optional ctor with parameters
            if (DefaultConstructArgs != null && DefaultConstructArgs.Length > 0)
            {
                var ctor = type.GetConstructor(DefaultConstructArgTypes);
                if (ctor != null) return ctor.Invoke(DefaultConstructArgs);
            }
            // Parameterless ctor
            return type.GetConstructor(Type.EmptyTypes)?.Invoke(null);
        }
    }
}