using System;
using System.Reflection;

namespace BH.SDK.Rules.Attributes
{
    public abstract class BaseRuleAttribute : Attribute
    {
        public const AttributeTargets ClassTarget = AttributeTargets.Class;
        public const AttributeTargets PropertyTarget = AttributeTargets.Property;

        protected virtual bool IsValidTypeInternal(PropertyInfo property) => true;
        protected abstract bool IsValidInternal(object value, object context);
        protected abstract void FixInternal(object target, PropertyInfo property, object context);

        public virtual RuleGroup Group => RuleGroup.Error;
        public virtual bool HasIsValid => true;
        public virtual bool HasFix => true;

        public bool IsValidType(PropertyInfo property)
        {
            return property != null && IsValidTypeInternal(property);
        }
        public bool IsValid(object value, object context)
        {
            if (!HasIsValid) return true; // by default any property is valid
            if (value == null) return false;
            return IsValidInternal(value, context);
        }
        public void Fix(object target, PropertyInfo property, object context)
        {
            if (!HasFix) return;
            if (target == null || property == null || !property.CanWrite) return;
            FixInternal(target, property, context);
        }
    }
}