using System;
using System.Globalization;
using System.Reflection;

namespace BH.SDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleMaxAttribute : BaseRuleAttribute
    {
        public object Max { get; set; } // always include
        public object DefaultValue { get; set; }

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

        public RuleMaxAttribute(byte max, byte defaultValue) { Max = max; DefaultValue = defaultValue; }
        public RuleMaxAttribute(sbyte max, sbyte defaultValue) { Max = max; DefaultValue = defaultValue; }
        public RuleMaxAttribute(short max, short defaultValue) { Max = max; DefaultValue = defaultValue; }
        public RuleMaxAttribute(ushort max, ushort defaultValue) { Max = max; DefaultValue = defaultValue; }
        public RuleMaxAttribute(int max, int defaultValue) { Max = max; DefaultValue = defaultValue; }
        public RuleMaxAttribute(uint max, uint defaultValue) { Max = max; DefaultValue = defaultValue; }
        public RuleMaxAttribute(long max, long defaultValue) { Max = max; DefaultValue = defaultValue; }
        public RuleMaxAttribute(ulong max, ulong defaultValue) { Max = max; DefaultValue = defaultValue; }
        public RuleMaxAttribute(float max, float defaultValue) { Max = max; DefaultValue = defaultValue; }
        public RuleMaxAttribute(double max, double defaultValue) { Max = max; DefaultValue = defaultValue; }
        public RuleMaxAttribute(decimal max, decimal defaultValue) { Max = max; DefaultValue = defaultValue; }
        public RuleMaxAttribute(object max, object defaultValue) { Max = max; DefaultValue = defaultValue; }
        
        protected override bool IsValidInternal(object value, object context)
        {
            if (Max == null) return false;

            var type = value.GetType();
            var max = ConvertBoundary(type, Max);
            var convertedValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);

            if (convertedValue is not IComparable comparableValue) return false;
            
            return comparableValue.CompareTo(max) <= 0;
        }

        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            if (Max == null) return;

            var value = property.GetValue(target);
            if (value == null) return;

            var type = property.PropertyType;
            var max = ConvertBoundary(type, Max);
            var convertedValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);

            if (convertedValue is not IComparable comparableValue) return;
            
            if (comparableValue.CompareTo(max) > 0)
            {
                var newValue = DefaultValue ?? max;
                property.SetValue(target, newValue);
            }
        }
        
        private static object ConvertBoundary(Type targetType, object boundary)
        {
            return targetType.IsInstanceOfType(boundary)
                ? boundary : Convert.ChangeType(boundary, targetType, CultureInfo.InvariantCulture);
        }
    }
}