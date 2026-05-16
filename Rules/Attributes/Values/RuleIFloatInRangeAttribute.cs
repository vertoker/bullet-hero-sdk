using System;
using System.Reflection;
using BHSDK.Models;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Utils;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleIFloatInRangeAttribute : BaseRuleAttribute
    {
        // always include
        public float Min { get; set; }
        public float Max { get; set; }
        
        public float Diff => Max - Min;
        public float HalfDiff => (Max - Min) / 2f;

        public RuleIFloatInRangeAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }

        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(IFloat).IsAssignableFrom(property.PropertyType);
        
        protected override bool IsValidInternal(object value, object context)
        {
            if (value is not IFloat flt) return false;

            switch (flt.GetModelType())
            {
                case FloatType.Value:
                {
                    var floatValue = (FloatValue)value;
                    if (floatValue.Value < Min || floatValue.Value > Max) return false;
                    return true;
                }
                case FloatType.RandomMinMax:
                {
                    var floatRandomMinMax = (FloatMinMax)value;
                    if (floatRandomMinMax.Min < Min || floatRandomMinMax.Max > Max) return false;
                    return true;
                }
                case FloatType.RandomMinMaxStep:
                {
                    var floatRandomMinMaxStep = (FloatMinMaxStep)value;
                    if (floatRandomMinMaxStep.Min < Min || floatRandomMinMaxStep.Max > Max) return false;
                    return true;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            var value = property.GetValue(target);
            if (value is not IFloat flt) return;

            switch (flt.GetModelType())
            {
                case FloatType.Value:
                {
                    var floatValue = (FloatValue)value;
                    if (floatValue.Value < Min || floatValue.Value > Max)
                        floatValue.Value = MathUtils.Clamp(floatValue.Value, Min, Max);
                    break;
                }
                case FloatType.RandomMinMax:
                {
                    var floatRandomMinMax = (FloatMinMax)value;
                    if (floatRandomMinMax.Min < Min) floatRandomMinMax.Min = Min;
                    if (floatRandomMinMax.Max > Max) floatRandomMinMax.Max = Max;
                    break;
                }
                case FloatType.RandomMinMaxStep:
                {
                    var floatRandomMinMaxStep = (FloatMinMaxStep)value;
                    if (floatRandomMinMaxStep.Min < Min) floatRandomMinMaxStep.Min = Min;
                    if (floatRandomMinMaxStep.Max > Max) floatRandomMinMaxStep.Max = Max;
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}