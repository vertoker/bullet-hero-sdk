using System;
using System.Reflection;
using BHSDK.Models;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleGuidNotEmptyAttribute : BaseRuleAttribute
    {
        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(Guid).IsAssignableFrom(property.PropertyType);
        
        protected override bool IsValidInternal(object value, Level context)
            => value is Guid guid && guid != Guid.Empty;
        
        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            property.SetValue(target, Guid.NewGuid());
        }
    }
}