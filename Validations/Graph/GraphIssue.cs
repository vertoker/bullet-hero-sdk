using BH.SDK.Rules;

namespace BH.SDK.Validations.Graph
{
    // Deliberately NOT a RuleIssue. A RuleIssue is addressed by a RulePath - a chain of properties
    // and collection keys ending at one value - which is exactly what a graph finding is not: "these
    // two objects share an id", "this chain loops back on itself" have no single property to point
    // at, and no property to write a repair into. Forcing them into that shape would produce issues
    // whose Fix could not run and whose path lied about where the problem was.
    //
    // None of these carries a fix at all. Every repair here is a content decision - which of two
    // colliding objects keeps its id, where a broken chain should reattach - and guessing would
    // silently rewrite the author's level.

    /// <summary> One violated cross-object invariant: what broke, how badly, and where. </summary>
    public readonly struct GraphIssue
    {
        public readonly GraphRule Rule;
        public readonly RuleGroup Group;

        /// <summary> Human-readable location - a scope plus the ids involved, since there is no
        /// property path to give. </summary>
        public readonly string Path;

        public readonly string Message;

        public GraphIssue(GraphRule rule, RuleGroup group, string path, string message)
        {
            Rule = rule;
            Group = group;
            Path = path;
            Message = message;
        }

        public override string ToString() => $"Graph issue, Rule: {Rule}, At: {Path}, {Message}";
    }
}
