using System;
using System.Reflection;
using BHSDK.Models;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleStringMaxAttribute : BaseRuleAttribute
    {
        public int MaxLength { get; set; }

        public RuleStringMaxAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }
        
        protected override bool IsValidTypeInternal(object value) => value is string;
        
        protected override bool IsValidInternal(object value, Level context)
            => value is string str && str.Length <= MaxLength;

        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            if (property.PropertyType != typeof(string)) return;
            if (property.GetValue(target) is not string s) return;
            
            if (s.Length > MaxLength)
                property.SetValue(target, s.Substring(0, MaxLength));;
        }
    }
}