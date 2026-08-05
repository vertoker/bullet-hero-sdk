using System;

namespace BH.SDK.Rules.Attributes
{
    // Split into two contracts because rules answer two different questions:
    //
    // - BasePropertyRuleAttribute sees ONE value ("is this frame inside the timeline") and is what
    //   almost every rule needs.
    // - BaseObjectRuleAttribute sees the whole object ("is Min below Max", "is StartFrame below
    //   EndFrame") and exists because a property rule structurally cannot: it receives the value and
    //   the context, never the sibling it has to be compared against.
    //
    // This class holds only what both share, so RuleIssue and the severity filter can treat any rule
    // uniformly without knowing which kind it is.

    /// <summary> Base of every declarative rule, of either kind. </summary>
    public abstract class BaseRuleAttribute : Attribute
    {
        public const AttributeTargets ClassTarget = AttributeTargets.Class;
        public const AttributeTargets PropertyTarget = AttributeTargets.Property;

        /// <summary> How badly a violation breaks the level: Error means unplayable, Warning means
        /// playable but wrong, Advice means cosmetic. Drives RuleAnalyzerSettings' severity filter. </summary>
        public virtual RuleGroup Group => RuleGroup.Error;

        public virtual bool HasIsValid => true;
        public virtual bool HasFix => true;
    }
}
