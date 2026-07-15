using System;
using System.Reflection;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Rules.Attributes
{
    // Generic max-value check for any primitive-int struct (AudioId, ColliderId, TypedResourceId, ...)
    [AttributeUsage(PropertyTarget)]
    public class RuleIPrimitiveIntMaxAttribute : BaseRuleAttribute
    {
        // always include
        public int Max { get; set; }

        public RuleIPrimitiveIntMaxAttribute(int max)
        {
            Max = max;
        }

        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(IPrimitiveInt).IsAssignableFrom(property.PropertyType);

        protected override bool IsValidInternal(object value, object context)
            => value is IPrimitiveInt primitive && primitive.Value <= Max;

        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            if (property.GetValue(target) is not IPrimitiveInt primitive) return;
            if (primitive.Value <= Max) return;

            var fixedValue = Activator.CreateInstance(property.PropertyType, Max);
            property.SetValue(target, fixedValue);
        }
    }
}
