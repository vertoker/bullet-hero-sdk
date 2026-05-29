using System;
using System.Reflection;
using BH.SDK.Models;

namespace BH.SDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleObjectIdValidAttribute : BaseRuleAttribute
    {
        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(int).IsAssignableFrom(property.PropertyType);
        
        protected override bool IsValidInternal(object value, object context)
            => value is int objectId and >= IdRules.MinObjectId
               && context is Level level; // TODO add complex check for parenting and ids

        private static readonly Random Random = new(DateTime.Now.Millisecond); // TODO remove and make normal
        
        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            if (context is not Level level) return;
            
            var value = property.GetValue(target);
            if (value is not int objectId) return;

            if (objectId < IdRules.MinObjectId)
            {
                objectId = Random.Next(IdRules.MinObjectId, int.MaxValue);
                property.SetValue(target, objectId);
            }
        }
    }
}