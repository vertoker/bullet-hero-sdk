using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BHSDK.Models;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleCollectionUniqueAttribute : BaseRuleAttribute
    {
        public string ItemPropertyName { get; set; }

        public RuleCollectionUniqueAttribute()
        {
            ItemPropertyName = string.Empty;
        }
        public RuleCollectionUniqueAttribute(string itemPropertyName)
        {
            ItemPropertyName = itemPropertyName;
        }

        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(ICollection).IsAssignableFrom(property.PropertyType);
        
        protected override bool IsValidInternal(object value, object context)
        {
            if (value is not ICollection collection) return false;

            var set = new HashSet<object>();
            PropertyInfo propertyInfo = null;

            foreach (var item in collection)
            {
                if (item == null) continue;

                object key;
                if (!string.IsNullOrEmpty(ItemPropertyName))
                {
                    if (propertyInfo == null)
                    {
                        propertyInfo = item.GetType().GetProperty(ItemPropertyName);
                        if (propertyInfo == null) return false;
                    }
                    
                    key = propertyInfo.GetValue(item);
                    if (key == null) return false;
                }
                else
                {
                    key = item;
                }

                if (!set.Add(key)) return false;
            }
            return true;
        }

        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            var value = property.GetValue(target);
            if (value is not IList list || list.IsReadOnly) return;

            var set = new HashSet<object>();
            PropertyInfo propertyInfo = null;
            
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];
                if (item == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                object key;
                if (!string.IsNullOrEmpty(ItemPropertyName))
                {
                    if (propertyInfo == null)
                    {
                        propertyInfo = item.GetType().GetProperty(ItemPropertyName);
                        if (propertyInfo == null) return;
                    }
                    
                    key = propertyInfo.GetValue(item);
                    if (key == null)
                    {
                        list.RemoveAt(i);
                        continue;
                    }
                }
                else
                {
                    key = item;
                }

                if (!set.Add(key))
                    list.RemoveAt(i);
            }
        }
    }
}