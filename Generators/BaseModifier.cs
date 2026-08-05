namespace BH.SDK.Generators
{
    /// <summary>
    /// Base for a generator that EDITS content that already exists. Requirements default to
    /// Selection here rather than None - a modifier that works on the whole scope regardless of
    /// what's selected is the exception, and it overrides this explicitly.
    /// </summary>
    public abstract class BaseModifier<TParams> : BaseScopeGenerator<TParams> where TParams : class, new()
    {
        public sealed override GeneratorKind Kind => GeneratorKind.Modifier;

        public override GeneratorRequirements Requirements => GeneratorRequirements.Selection;
    }
}
