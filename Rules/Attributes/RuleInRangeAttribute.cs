using System;
using System.Globalization;
using System.Reflection;

namespace BH.SDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleInRangeAttribute : BaseRuleAttribute
    {
        public object Min { get; set; } // always include
        public object Max { get; set; } // always include
        public object DefaultValue { get; set; }

        public RuleInRangeAttribute(byte min, byte max) { Min = min; Max = max; }
        public RuleInRangeAttribute(sbyte min, sbyte max) { Min = min; Max = max; }
        public RuleInRangeAttribute(short min, short max) { Min = min; Max = max; }
        public RuleInRangeAttribute(ushort min, ushort max) { Min = min; Max = max; }
        public RuleInRangeAttribute(int min, int max) { Min = min; Max = max; }
        public RuleInRangeAttribute(uint min, uint max) { Min = min; Max = max; }
        public RuleInRangeAttribute(long min, long max) { Min = min; Max = max; }
        public RuleInRangeAttribute(ulong min, ulong max) { Min = min; Max = max; }
        public RuleInRangeAttribute(float min, float max) { Min = min; Max = max; }
        public RuleInRangeAttribute(double min, double max) { Min = min; Max = max; }
        public RuleInRangeAttribute(decimal min, decimal max) { Min = min; Max = max; }
        public RuleInRangeAttribute(object min, object max) { Min = min; Max = max; }

        public RuleInRangeAttribute(byte min, byte max, byte defaultValue)
            { Min = min; Max = max; DefaultValue = defaultValue; }
        public RuleInRangeAttribute(sbyte min, sbyte max, sbyte defaultValue)
            { Min = min; Max = max; DefaultValue = defaultValue; }
        public RuleInRangeAttribute(short min, short max, short defaultValue)
            { Min = min; Max = max; DefaultValue = defaultValue; }
        public RuleInRangeAttribute(ushort min, ushort max, ushort defaultValue)
            { Min = min; Max = max; DefaultValue = defaultValue; }
        public RuleInRangeAttribute(int min, int max, int defaultValue)
            { Min = min; Max = max; DefaultValue = defaultValue; }
        public RuleInRangeAttribute(uint min, uint max, uint defaultValue)
            { Min = min; Max = max; DefaultValue = defaultValue; }
        public RuleInRangeAttribute(long min, long max, long defaultValue)
            { Min = min; Max = max; DefaultValue = defaultValue; }
        public RuleInRangeAttribute(ulong min, ulong max, ulong defaultValue)
            { Min = min; Max = max; DefaultValue = defaultValue; }
        public RuleInRangeAttribute(float min, float max, float defaultValue)
            { Min = min; Max = max; DefaultValue = defaultValue; }
        public RuleInRangeAttribute(double min, double max, double defaultValue)
            { Min = min; Max = max; DefaultValue = defaultValue; }
        public RuleInRangeAttribute(decimal min, decimal max, decimal defaultValue)
            { Min = min; Max = max; DefaultValue = defaultValue; }
        public RuleInRangeAttribute(object min, object max, object defaultValue)
            { Min = min; Max = max; DefaultValue = defaultValue; }
        
        protected override bool IsValidInternal(object value, object context)
        {
            if (Min == null || Max == null) return false;

            var type = value.GetType();
            var min = ConvertBoundary(type, Min);
            var max = ConvertBoundary(type, Max);
            var convertedValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);

            if (convertedValue is not IComparable comparableValue) return false;
            
            return comparableValue.CompareTo(min) >= 0 && comparableValue.CompareTo(max) <= 0;
        }

        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            if (Min == null || Max == null) return;

            var value = property.GetValue(target);
            if (value == null) return;

            var type = property.PropertyType;
            var min = ConvertBoundary(type, Min);
            var max = ConvertBoundary(type, Max);
            var convertedValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);

            if (convertedValue is not IComparable comparableValue) return;
            
            if (comparableValue.CompareTo(min) < 0)
            {
                var newValue = DefaultValue ?? min;
                property.SetValue(target, newValue);
            }
            else if (comparableValue.CompareTo(max) > 0)
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