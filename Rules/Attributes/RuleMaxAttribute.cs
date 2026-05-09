using System;
using System.Globalization;
using System.Reflection;
using BHSDK.Models;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleMaxAttribute : BaseRuleAttribute
    {
        public object Max { get; set; }

        public RuleMaxAttribute(byte max) { Max = max; }
        public RuleMaxAttribute(sbyte max) { Max = max; }
        public RuleMaxAttribute(short max) { Max = max; }
        public RuleMaxAttribute(ushort max) { Max = max; }
        public RuleMaxAttribute(int max) { Max = max; }
        public RuleMaxAttribute(uint max) { Max = max; }
        public RuleMaxAttribute(long max) { Max = max; }
        public RuleMaxAttribute(ulong max) { Max = max; }
        public RuleMaxAttribute(float max) { Max = max; }
        public RuleMaxAttribute(double max) { Max = max; }
        public RuleMaxAttribute(decimal max) { Max = max; }
        public RuleMaxAttribute(object max) { Max = max; }
        
        protected override bool IsValidInternal(object value, Level context)
        {
            if (Max == null) return false;

            var type = value.GetType();
            var max = ConvertBoundary(type, Max);
            var convertedValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);

            if (convertedValue is not IComparable comparableValue) return false;
            
            return comparableValue.CompareTo(max) <= 0;
        }

        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            if (Max == null) return;

            var value = property.GetValue(target);
            if (value == null) return;

            var type = property.PropertyType;
            var max = ConvertBoundary(type, Max);
            var convertedValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);

            if (convertedValue is not IComparable comparableValue) return;
            
            if (comparableValue.CompareTo(max) > 0)
                property.SetValue(target, max);
        }
        
        private static object ConvertBoundary(Type targetType, object boundary)
        {
            return targetType.IsInstanceOfType(boundary)
                ? boundary : Convert.ChangeType(boundary, targetType, CultureInfo.InvariantCulture);
        }
    }
}