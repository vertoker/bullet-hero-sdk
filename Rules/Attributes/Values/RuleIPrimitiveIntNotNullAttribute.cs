using System;
using System.Reflection;
using BH.SDK.Models.Interfaces.Primitives;

namespace BH.SDK.Rules.Attributes
{
    // Generic "is set" check for any primitive-int struct (ColliderId, ThemeId, TypedResourceId, ...):
    // value must not be the reserved NullValue (always 0 by convention). Unlike
    // RuleIPrimitiveIntMin/Max, this does not restrict the value to a game-defined or user-defined
    // subrange - use this when a property may reference either range, just not "unset".
    [AttributeUsage(PropertyTarget)]
    public class RuleIPrimitiveIntNotNullAttribute : BasePropertyRuleAttribute
    {
        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(IPrimitiveInt).IsAssignableFrom(property.PropertyType);

        protected override bool IsValidInternal(object value, RuleContext context)
            => value is IPrimitiveInt primitive && primitive.Value != 0;

        protected override void FixInternal(object target, PropertyInfo property, RuleContext context)
        {
            if (property.GetValue(target) is not IPrimitiveInt primitive) return;
            if (primitive.Value != 0) return;

            var fixedValue = Activator.CreateInstance(property.PropertyType, 1);
            property.SetValue(target, fixedValue);
        }
    }
}
