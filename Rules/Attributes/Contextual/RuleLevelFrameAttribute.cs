using System;
using System.Reflection;
using BH.SDK.Models;
using BH.SDK.Utils;

namespace BH.SDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleLevelFrameAttribute : BaseRuleAttribute
    {
        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(int).IsAssignableFrom(property.PropertyType);

        protected override bool IsValidInternal(object value, object context)
            => value is int frame and >= FrameRules.MinFrame
               && context is Level level && frame < level.Settings.FrameLength;

        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            if (context is not Level level) return;
            if (level.Settings.FrameLength < FrameRules.MinFrame) return;
            
            var value = property.GetValue(target);
            if (value is not int frame) return;

            if (frame < FrameRules.MinFrame || frame >= level.Settings.FrameLength)
            {
                frame = MathUtils.Clamp(frame, FrameRules.MinFrame, level.Settings.FrameLength - 1);
                property.SetValue(target, frame);
            }
        }
    }
}