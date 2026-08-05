using System;
using System.Reflection;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Rules.Attributes
{
    // Generic min-value check for any primitive-int struct (AudioId, ColliderId, TypedResourceId, ...)
    [AttributeUsage(PropertyTarget)]
    public class RuleIPrimitiveIntMinAttribute : BasePropertyRuleAttribute
    {
        // always include
        public int Min { get; set; }

        public RuleIPrimitiveIntMinAttribute(int min)
        {
            Min = min;
        }

        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(IPrimitiveInt).IsAssignableFrom(property.PropertyType);

        protected override bool IsValidInternal(object value, RuleContext context)
            => value is IPrimitiveInt primitive && primitive.Value >= Min;

        protected override void FixInternal(object target, PropertyInfo property, RuleContext context)
        {
            if (property.GetValue(target) is not IPrimitiveInt primitive) return;
            if (primitive.Value >= Min) return;

            var fixedValue = Activator.CreateInstance(property.PropertyType, Min);
            property.SetValue(target, fixedValue);
        }
    }
}
