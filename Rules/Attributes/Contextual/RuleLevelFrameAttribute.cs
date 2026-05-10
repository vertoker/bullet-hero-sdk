using System;
using System.Reflection;
using BHSDK.Models;
using BHSDK.Utils;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleLevelFrameAttribute : BaseRuleAttribute
    {
        protected override bool IsValidTypeInternal(object value) => value is int;

        protected override bool IsValidInternal(object value, Level context)
            => value is int frame and >= FrameRules.MinFrame && frame < context.Settings.FrameLength;

        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            if (context.Settings.FrameLength < FrameRules.MinFrame) return;
            
            var value = property.GetValue(target);
            if (value is not int frame) return;

            if (frame < FrameRules.MinFrame || frame >= context.Settings.FrameLength)
            {
                frame = MathUtils.Clamp(frame, FrameRules.MinFrame, context.Settings.FrameLength);
                property.SetValue(target, frame);
            }
        }
    }
}