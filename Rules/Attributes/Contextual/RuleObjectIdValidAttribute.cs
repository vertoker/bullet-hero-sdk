using System;
using System.Reflection;
using BHSDK.Models;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleObjectIdValidAttribute : BaseRuleAttribute
    {
        protected override bool IsValidTypeInternal(object value) => value is int;
        
        protected override bool IsValidInternal(object value, Level context)
        {
            throw new NotImplementedException();
        }

        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            throw new NotImplementedException();
        }
    }
}