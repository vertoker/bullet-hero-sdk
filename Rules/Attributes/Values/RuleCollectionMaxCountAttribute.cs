using System;
using System.Collections;
using System.Reflection;

namespace BH.SDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleCollectionMaxCountAttribute : BasePropertyRuleAttribute
    {
        public int MaxCount { get; set; }

        public RuleCollectionMaxCountAttribute(int maxCount)
        {
            MaxCount = maxCount;
        }

        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(ICollection).IsAssignableFrom(property.PropertyType);

        protected override bool IsValidInternal(object value, RuleContext context)
            => value is ICollection col && col.Count <= MaxCount;
        
        protected override void FixInternal(object target, PropertyInfo property, RuleContext context)
        {
            var value = property.GetValue(target);
            if (value == null) return;

            // Array before IList, same reason as in RuleCollectionCount: an array is an IList too,
            // but a fixed-size one, so trimming it means replacing it through the property setter.
            if (value is Array array)
            {
                FixArray(array, property, target);
            }
            else if (value is IList list)
            {
                FixList(list, property);
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