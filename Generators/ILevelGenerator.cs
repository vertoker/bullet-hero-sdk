namespace BH.SDK.Generators
{
    /// <summary>
    /// A generator that builds a whole level from nothing. It runs before any level is open, so it
    /// gets no GeneratorContext and produces no change log - "undo" for this is not creating the
    /// level in the first place.
    /// </summary>
    public interface ILevelGenerator : IGenerator
    {
        GeneratedLevel Create(object parameters);
    }
}
