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
    }
}
