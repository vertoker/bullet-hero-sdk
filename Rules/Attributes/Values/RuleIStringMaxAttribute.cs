using System;
using System.Linq;
using System.Reflection;
using BHSDK.Models;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleIStringMaxAttribute : BaseRuleAttribute
    {
        public int MaxLength { get; set; }

        public RuleIStringMaxAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }

        protected override bool IsValidTypeInternal(object value) => value is IString;
        
        protected override bool IsValidInternal(object value, Level context)
        {
            if (value is not IString str) return false;

            switch (str.GetModelType())
            {
                case StringType.Value:
                {
                    var valueValue = (StringValue)value;
                    return valueValue.Value.Length <= MaxLength;
                }
                case StringType.Localized:
                {
                    var localizedValue = (StringLocalized)value;
                    foreach (var stringLanguage in localizedValue.Strings)
                    {
                        if (stringLanguage.Value.Length > MaxLength) 
                            return false;
                    }
                    return true;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            var value = property.GetValue(target);
            if (value is not IString str) return;
            
            switch (str.GetModelType())
            {
                case StringType.Value:
                {
                    var valueValue = (StringValue)value;
                    valueValue.Value = valueValue.Value[..MaxLength];
                    break;
                }
                case StringType.Localized:
                {
                    var localizedValue = (StringLocalized)value;
                    foreach (var stringLanguage in localizedValue.Strings)
                    {
                        if (stringLanguage.Value.Length > MaxLength) 
                            stringLanguage.Value = stringLanguage.Value[..MaxLength];
                    }
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}