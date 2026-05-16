using System;
using System.Reflection;
using BHSDK.Models;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleNotNullAttribute : BaseRuleAttribute
    {
        public Type DefaultConstructType { get; set; }

        public RuleNotNullAttribute()
        {
            
        }
        public RuleNotNullAttribute(Type defaultConstructType)
        {
            DefaultConstructType = defaultConstructType;
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
        
        private static object CreateDefaultValue(Type type)
        {
            // struct
            if (type.IsValueType) return Activator.CreateInstance(type);
            
            // class
            return type.GetConstructor(Type.EmptyTypes)?.Invoke(null);
        }
    }
}