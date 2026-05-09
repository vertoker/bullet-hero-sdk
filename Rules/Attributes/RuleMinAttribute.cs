using System;
using System.Globalization;
using System.Reflection;
using BHSDK.Models;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleMinAttribute : BaseRuleAttribute
    {
        // always include
        public object Min { get; set; }

        public RuleMinAttribute(byte min) { Min = min; }
        public RuleMinAttribute(sbyte min) { Min = min; }
        public RuleMinAttribute(short min) { Min = min; }
        public RuleMinAttribute(ushort min) { Min = min; }
        public RuleMinAttribute(int min) { Min = min; }
        public RuleMinAttribute(uint min) { Min = min; }
        public RuleMinAttribute(long min) { Min = min; }
        public RuleMinAttribute(ulong min) { Min = min; }
        public RuleMinAttribute(float min) { Min = min; }
        public RuleMinAttribute(double min) { Min = min; }
        public RuleMinAttribute(decimal min) { Min = min; }
        public RuleMinAttribute(object min) { Min = min; }
        
        protected override bool IsValidInternal(object value, Level context)
        {
            if (Min == null) return false;

            var type = value.GetType();
            var min = ConvertBoundary(type, Min);
            var convertedValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);

            if (convertedValue is not IComparable comparableValue) return false;
            
            return comparableValue.CompareTo(min) >= 0;
        }

        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            if (Min == null) return;

            var value = property.GetValue(target);
            if (value == null) return;

            var type = property.PropertyType;
            var min = ConvertBoundary(type, Min);
            var convertedValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);

            if (convertedValue is not IComparable comparableValue) return;
            
            if (comparableValue.CompareTo(min) < 0)
                property.SetValue(target, min);
        }
        
        private static object ConvertBoundary(Type targetType, object boundary)
        {
            return targetType.IsInstanceOfType(boundary)
                ? boundary : Convert.ChangeType(boundary, targetType, CultureInfo.InvariantCulture);
        }
    }
}