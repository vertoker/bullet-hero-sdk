using System;
using System.Reflection;
using BHSDK.Models;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleNotNullAttribute : BaseRuleAttribute
    {
        protected override bool IsValidInternal(object value, Level context)
        {
            return value != null;
        }
        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            var value = Activator.CreateInstance(property.PropertyType);
            property.SetValue(target, value);
        }
    }
}