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
    public class RuleIIntInRangeAttribute : BaseRuleAttribute
    {
        // always include
        public int Min { get; set; }
        public int Max { get; set; }
        
        public int Diff => Max - Min;
        public int HalfDiff => (Max - Min) / 2;

        public RuleIIntInRangeAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }

        protected override bool IsValidTypeInternal(object value) => value is IInt;
        
        protected override bool IsValidInternal(object value, Level context)
        {
            if (value is not IInt integer) return false;

            switch (integer.GetModelType())
            {
                case IntType.Value:
                {
                    var intValue = (IntValue)value;
                    if (intValue.Value < Min || intValue.Value > Max) return false;
                    return true;
                }
                case IntType.RandomMinMax:
                {
                    var intRandomMinMax = (IntMinMax)value;
                    if (intRandomMinMax.Min < Min || intRandomMinMax.Max > Max) return false;
                    return true;
                }
                case IntType.RandomMinMaxStep:
                {
                    var intRandomMinMaxStep = (IntMinMaxStep)value;
                    if (intRandomMinMaxStep.Min < Min || intRandomMinMaxStep.Max > Max) return false;
                    return true;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            var value = property.GetValue(target);
            if (value is not IInt integer) return;

            switch (integer.GetModelType())
            {
                case IntType.Value:
                {
                    var intValue = (IntValue)value;
                    if (intValue.Value < Min || intValue.Value > Max)
                        intValue.Value = MathUtils.Clamp(intValue.Value, Min, Max);
                    break;
                }
                case IntType.RandomMinMax:
                {
                    var intRandomMinMax = (IntMinMax)value;
                    if (intRandomMinMax.Min < Min) intRandomMinMax.Min = Min;
                    if (intRandomMinMax.Max > Max) intRandomMinMax.Max = Max;
                    break;
                }
                case IntType.RandomMinMaxStep:
                {
                    var intRandomMinMaxStep = (IntMinMaxStep)value;
                    if (intRandomMinMaxStep.Min < Min) intRandomMinMaxStep.Min = Min;
                    if (intRandomMinMaxStep.Max > Max) intRandomMinMaxStep.Max = Max;
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}