using System;
using System.Reflection;
using BHSDK.Models;
using BHSDK.Utils;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleThemeIndexAttribute : BaseRuleAttribute
    {
        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(int).IsAssignableFrom(property.PropertyType);

        protected override bool IsValidInternal(object value, Level context)
            => value is int themeIndex and >= 0 && themeIndex < context.Game.Themes.Count;

        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            if (context.Game.Themes.Count == 0) return;
            
            var value = property.GetValue(target);
            if (value is not int themeIndex) return;

            if (themeIndex < 0 || themeIndex >= context.Game.Themes.Count)
            {
                themeIndex = MathUtils.Clamp(themeIndex, 0, context.Game.Themes.Count);
                property.SetValue(target, themeIndex);
            }
        }
    }
}