using System.Reflection;

namespace BH.SDK.Rules.Attributes
{
    /// <summary>
    /// A rule about one property's value: can this rule sit on this property, does the value satisfy
    /// it, and how to repair one that does not. Rules see their surroundings only through
    /// RuleContext - never the raw analysis root - so the same rule works at level scope and inside
    /// a prefab template without knowing which it is in.
    /// </summary>
    public abstract class BasePropertyRuleAttribute : BaseRuleAttribute
    {
        protected abstract bool IsValidTypeInternal(PropertyInfo property);
        protected abstract bool IsValidInternal(object value, RuleContext context);
        protected abstract void FixInternal(object target, PropertyInfo property, RuleContext context);

        public bool IsValidType(PropertyInfo property)
        {
            return property != null && IsValidTypeInternal(property);
        }
        public bool IsValid(object value, RuleContext context)
        {
            if (!HasIsValid) return true; // by default any property is valid
            if (value == null) return false;
            return IsValidInternal(value, context);
        }
        public void Fix(object target, PropertyInfo property, RuleContext context)
        {
            if (!HasFix) return;
            if (target == null || property == null || !property.CanWrite) return;
            FixInternal(target, property, context);
        }
    }
}
