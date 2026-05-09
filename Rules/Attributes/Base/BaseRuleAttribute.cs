using System;
using System.Reflection;
using BHSDK.Models;

namespace BHSDK.Rules.Attributes
{
    public abstract class BaseRuleAttribute : Attribute
    {
        public const AttributeTargets ClassTarget = AttributeTargets.Class;
        public const AttributeTargets PropertyTarget = AttributeTargets.Property;

        protected virtual bool IsValidTypeInternal(object value) => true;
        protected abstract bool IsValidInternal(object value, Level context);
        protected abstract void FixInternal(object target, PropertyInfo property, Level context);

        public virtual bool HasIsValid => true;
        public virtual bool HasFix => true;

        public bool IsValidType(object value)
        {
            return value != null && IsValidTypeInternal(value);
        }
        public bool IsValid(object value, Level context)
        {
            if (!HasIsValid) return true; // by default any property is valid
            if (value == null) return false;
            return IsValidInternal(value, context);
        }
        public void Fix(object target, PropertyInfo property, Level context)
        {
            if (!HasFix) return;
            if (target == null || property == null || !property.CanWrite) return;
            FixInternal(target, property, context);
        }
    }
}