using System;
using System.Collections;
using System.Reflection;
using BHSDK.Models;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleCollectionMaxCountAttribute : BaseRuleAttribute
    {
        public int MaxCount { get; set; }

        public RuleCollectionMaxCountAttribute(int maxCount)
        {
            MaxCount = maxCount;
        }

        protected override bool IsValidTypeInternal(object value) => value is ICollection;

        protected override bool IsValidInternal(object value, Level context)
            => value is ICollection col && col.Count <= MaxCount;
        
        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            var value = property.GetValue(target);
            if (value == null) return;

            if (value is IList list)
            {
                FixList(list, property);
            }
            else if (property.PropertyType.IsArray)
            {
                FixArray((Array)value, property, target);
            }
        }

        private void FixList(IList list, PropertyInfo property)
        {
            if (list.Count > MaxCount)
            {
                while (list.Count > MaxCount)
                    list.RemoveAt(list.Count - 1);
            }
        }
        private void FixArray(Array array, PropertyInfo property, object target)
        {
            var length = array.Length;
            if (length == MaxCount) return;

            var elementType = property.PropertyType.GetElementType();
            var newArray = Array.CreateInstance(elementType, MaxCount);

            var copyLength = Math.Min(length, MaxCount);
            if (copyLength > 0) Array.Copy(array, newArray, copyLength);
            
            property.SetValue(target, newArray);
        }
    }
}