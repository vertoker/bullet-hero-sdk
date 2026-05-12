using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using BHSDK.Models;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleCollectionSortedAttribute : BaseRuleAttribute
    {
        public string ItemPropertyName { get; set; }

        public RuleCollectionSortedAttribute()
        {
            ItemPropertyName = string.Empty;
        }
        public RuleCollectionSortedAttribute(string itemPropertyName)
        {
            ItemPropertyName = itemPropertyName;
        }

        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(ICollection).IsAssignableFrom(property.PropertyType);
        
        protected override bool IsValidInternal(object value, Level context)
        {
            if (value is not ICollection collection) return false;
            
            IComparable previous = null;
            var hasPrevious = false;
            PropertyInfo propertyInfo = null;

            foreach (var item in collection)
            {
                if (item == null) return false;

                object compareValue;
                if (!string.IsNullOrEmpty(ItemPropertyName))
                {
                    if (propertyInfo == null)
                    {
                        propertyInfo = item.GetType().GetProperty(ItemPropertyName);
                        if (propertyInfo == null) return false;
                    }
                    
                    compareValue = propertyInfo.GetValue(item);
                    if (compareValue == null) return false;
                }
                else
                {
                    compareValue = item;
                }
                
                if (compareValue is not IComparable comparable) return false;

                if (hasPrevious)
                {
                    if (previous.CompareTo(comparable) > 0) return false;
                }

                previous = comparable;
                hasPrevious = true;
            }

            return true;
        }

        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            var value = property.GetValue(target);
            if (value is not ICollection collection) return;
            
            PropertyInfo propInfo = null;
            if (!string.IsNullOrEmpty(ItemPropertyName))
            {
                foreach (var item in collection)
                {
                    if (item == null) continue;
                    propInfo = item.GetType().GetProperty(ItemPropertyName);
                    break;
                }
                if (propInfo == null) return;
            }
            
            Comparison<object> comparer = (x, y) =>
            {
                object valX = x, valY = y;
                if (propInfo != null)
                {
                    valX = propInfo.GetValue(x);
                    valY = propInfo.GetValue(y);
                }

                if (valX == null && valY == null) return 0;
                if (valX == null) return -1;
                if (valY == null) return 1;

                return valX is IComparable compX ? compX.CompareTo(valY) : 0;
            };
            
            var collectionType = collection.GetType();
            
            if (collection is IList list && !list.IsReadOnly)
            {
                SortList(list, comparer);
            }
            else if (collectionType.IsArray)
            {
                SortArray((Array)collection, comparer);
            }
        }
        
        private static void SortList(IList list, Comparison<object> comparer)
        {
            var items = list.Cast<object>().ToList();
            items.Sort(comparer);
            
            list.Clear();
            foreach (var item in items) list.Add(item);
        }
        private static void SortArray(Array array, Comparison<object> comparer)
        {
            if (array.Length <= 1) return;

            var items = array.Cast<object>().ToList();
            items.Sort(comparer);
            
            for (var i = 0; i < items.Count; i++)
                array.SetValue(items[i], i);
        }
    }
}