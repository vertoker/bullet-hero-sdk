using System;
using System.Reflection;
using BH.SDK.Models;
using BH.SDK.Utils;

namespace BH.SDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleThemeIndexAttribute : BaseRuleAttribute
    {
        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(int).IsAssignableFrom(property.PropertyType);

        protected override bool IsValidInternal(object value, object context)
            => value is int themeIndex and >= 0
               && context is Level level && themeIndex < level.Game.Themes.Count;

        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            if (context is not Level level) return;
            if (level.Game.Themes.Count == 0) return;
            
            var value = property.GetValue(target);
            if (value is not int themeIndex) return;

            if (themeIndex < 0 || themeIndex >= level.Game.Themes.Count)
            {
                themeIndex = MathUtils.Clamp(themeIndex, 0, level.Game.Themes.Count);
                property.SetValue(target, themeIndex);
            }
        }
    }
}