using System;
using System.Reflection;

namespace BH.SDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleObjectIdValidAttribute : BaseRuleAttribute
    {
        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(int).IsAssignableFrom(property.PropertyType);
        
        protected override bool IsValidInternal(object value, object context)
        {
            throw new NotImplementedException();
        }

        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            throw new NotImplementedException();
        }
    }
}