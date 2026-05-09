using System;
using System.Reflection;
using BHSDK.Models;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleGuidNotEmpty : BaseRuleAttribute
    {
        protected override bool IsValidTypeInternal(object value) => value is Guid;
        
        protected override bool IsValidInternal(object value, Level context)
            => value is Guid guid && guid != Guid.Empty;
        
        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            property.SetValue(target, Guid.NewGuid());
        }
    }
}