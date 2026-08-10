namespace BH.SDK.Generators
{
    /// <summary>
    /// A generator that writes into an existing scope - both Content and Modifier kinds. They share
    /// this one entry point because they differ in intent and Requirements, not in mechanism;
    /// splitting them would duplicate the whole context/journal path for nothing.
    /// </summary>
    public interface IScopeGenerator : IGenerator
    {
        GeneratorResult Run(GeneratorContext context, object parameters);

        // A host refuses a run whose whole estimate is zero, because for most generators that means
        // the parameters produce nothing and a button that silently does nothing is worse than a
        // refusal. GeneratorCost measures objects, keyframes and resources, so a generator that
        // writes something the struct cannot express - a bare advisory field, say - always estimates
        // zero and would be refused forever. It says so here rather than the host guessing from Kind.

        /// <summary> Whether an all-zero <see cref="GeneratorCost"/> is this generator's normal state
        /// rather than "these parameters produce nothing". </summary>
        bool AllowsEmptyRun { get; }
    }
}
