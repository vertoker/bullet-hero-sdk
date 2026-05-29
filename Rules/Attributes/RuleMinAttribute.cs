using System;
using System.Globalization;
using System.Reflection;

namespace BH.SDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleMinAttribute : BaseRuleAttribute
    {
        public object Min { get; set; } // always include
        public object DefaultValue { get; set; }

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
        
        public RuleMinAttribute(byte min, byte defaultValue) { Min = min; DefaultValue = defaultValue; }
        public RuleMinAttribute(sbyte min, sbyte defaultValue) { Min = min; DefaultValue = defaultValue; }
        public RuleMinAttribute(short min, short defaultValue) { Min = min; DefaultValue = defaultValue; }
        public RuleMinAttribute(ushort min, ushort defaultValue) { Min = min; DefaultValue = defaultValue; }
        public RuleMinAttribute(int min, int defaultValue) { Min = min; DefaultValue = defaultValue; }
        public RuleMinAttribute(uint min, uint defaultValue) { Min = min; DefaultValue = defaultValue; }
        public RuleMinAttribute(long min, long defaultValue) { Min = min; DefaultValue = defaultValue; }
        public RuleMinAttribute(ulong min, ulong defaultValue) { Min = min; DefaultValue = defaultValue; }
        public RuleMinAttribute(float min, float defaultValue) { Min = min; DefaultValue = defaultValue; }
        public RuleMinAttribute(double min, double defaultValue) { Min = min; DefaultValue = defaultValue; }
        public RuleMinAttribute(decimal min, decimal defaultValue) { Min = min; DefaultValue = defaultValue; }
        public RuleMinAttribute(object min, object defaultValue) { Min = min; DefaultValue = defaultValue; }
        
        protected override bool IsValidInternal(object value, object context)
        {
            if (Min == null) return false;

            var type = value.GetType();
            var min = ConvertBoundary(type, Min);
            var convertedValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);

            if (convertedValue is not IComparable comparableValue) return false;
            
            return comparableValue.CompareTo(min) >= 0;
        }

        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            if (Min == null) return;

            var value = property.GetValue(target);
            if (value == null) return;

            var type = property.PropertyType;
            var min = ConvertBoundary(type, Min);
            var convertedValue = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);

            if (convertedValue is not IComparable comparableValue) return;

            if (comparableValue.CompareTo(min) < 0)
            {
                var newValue = DefaultValue ?? min;
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