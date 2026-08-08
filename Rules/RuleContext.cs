using BH.SDK.Models;
using BH.SDK.Models.Interfaces;

namespace BH.SDK.Rules
{
    // Replaces the bare "object context" every rule used to receive, which in practice meant "the
    // analysis root, and you had better hope it is a Level". Two things were wrong with that:
    //
    // - Validating anything other than a Level (a Prefab, a LevelMeta, a UserSettings) made every
    //   contextual rule report false failures that no Fix could ever clear, because they all began
    //   with `context is Level`.
    // - Descending into a Prefab never changed the context, so a template's own frames were measured
    //   against the LEVEL's FrameDuration instead of the template's own.
    //
    // Instances are immutable: entering a nested scope produces a new context rather than mutating
    // this one, so a RuleIssue can hold on to the exact context its rule saw and RuleFixer can
    // replay the fix against it later.

    /// <summary>
    /// What a rule is allowed to know about its surroundings: the analysis root, the level it
    /// belongs to (when there is one), and the object scope and timeline currently being walked.
    /// </summary>
    public sealed class RuleContext
    {
        /// <summary> Object Analyze was called on. Never null. </summary>
        public object Root { get; }

        /// <summary> Level the walk started from, or null when validating a standalone aggregate.
        /// Only rules that resolve level-wide references need this; frame and id rules must use
        /// the scope fields below, which stay correct inside a prefab template. </summary>
        public Level Level { get; }

        /// <summary> Objects of the scope being walked - a level's own or one prefab template's. </summary>
        public IObjectScope Objects { get; }

        /// <summary> Timeline length of the scope being walked, in frames. </summary>
        public int FrameDuration { get; }

        /// <summary> Whether the current scope is a prefab template rather than the level itself.
        /// PrefabRoot as a parent only means anything here. </summary>
        public bool IsPrefabScope { get; }

        /// <summary> Whether a scope could be resolved at all. False for roots that carry no
        /// timeline (LevelMeta, UserSettings, a bare value model) - scope-dependent rules degrade to
        /// what they can still check on their own rather than failing outright. </summary>
        public bool HasScope { get; }

        private RuleContext(object root, Level level, IObjectScope objects,
            int frameDuration, bool isPrefabScope, bool hasScope)
        {
            Root = root;
            Level = level;
            Objects = objects;
            FrameDuration = frameDuration;
            IsPrefabScope = isPrefabScope;
            HasScope = hasScope;
        }

        /// <summary> Build the starting context for a root, resolving its scope if it has one. </summary>
        public static RuleContext ForRoot(object root)
        {
            switch (root)
            {
                case Level level:
                    return new RuleContext(level, level, level.Game,
                        level.Settings.FrameDuration, false, true);

                // A standalone template validates against its own timeline, with no level around it.
                case IFrameScope scope:
                    return new RuleContext(root, null, scope, scope.FrameDuration, true, true);

                default:
                    return new RuleContext(root, null, null, 0, false, false);
            }
        }

        /// <summary> Context for a nested scope reached from this one - keeps the level for
        /// reference lookups, swaps everything scope-local. </summary>
        public RuleContext WithScope(IFrameScope scope)
        {
            if (scope == null) return this;
            return new RuleContext(Root, Level, scope, scope.FrameDuration, true, true);
        }
    }
}
