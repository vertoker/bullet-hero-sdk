using System;

namespace BH.SDK.Rules.Attributes
{
    // Declared on the class, not on a property, because the invariant spans several of them. The
    // analyzer runs these once per visited object, before its properties - so a pair that is
    // reported as unordered is reported against the object that owns both halves, which is also the
    // object a Fix has to write back into.
    //
    // Kept deliberately small: anything needing more than one object (id uniqueness, dangling
    // references, parent cycles) is a graph invariant and does not belong here either.

    /// <summary>
    /// A rule about a whole object: an invariant between several of its properties, which a
    /// property rule structurally cannot express - it only ever receives one value.
    /// </summary>
    public abstract class BaseObjectRuleAttribute : BaseRuleAttribute
    {
        protected abstract bool IsValidTypeInternal(Type type);
        protected abstract bool IsValidInternal(object target, RuleContext context);
        protected abstract void FixInternal(object target, RuleContext context);

        public bool IsValidType(Type type)
        {
            return type != null && IsValidTypeInternal(type);
        }
        public bool IsValid(object target, RuleContext context)
        {
            if (!HasIsValid) return true;
            if (target == null) return false;
            return IsValidInternal(target, context);
        }
        public void Fix(object target, RuleContext context)
        {
            if (!HasFix) return;
            if (target == null) return;
            FixInternal(target, context);
        }
    }
}
