using System;

namespace BH.SDK.Generators
{
    // This exists to be non-generic. The previous design's CRTP constraint
    // (where TParams : BaseGenerator<TParams>.BaseInputParameters) made a common root impossible,
    // which made a registry impossible, which forced every host to hardcode every generator by
    // hand - the single reason adding one was expensive. Everything a host needs to LIST, LABEL and
    // build a FORM for a generator is on this interface; running it needs one of the two
    // sub-interfaces, picked by Kind.

    /// <summary>
    /// What every generator is, independent of what it generates.
    /// </summary>
    public interface IGenerator
    {
        /// <summary> Stable snake_case identity, mirroring LevelEditorOperation.OperationNameKey:
        /// "gen_level_*" for level generators, "gen_*" for content, "mod_*" for modifiers. Also the
        /// root of every label key ("{NameKey}_{field}"), and the only identity a user ever sees -
        /// class names are never shown. </summary>
        string NameKey { get; }

        GeneratorKind Kind { get; }

        /// <summary> What the host must provide before this can run. </summary>
        GeneratorRequirements Requirements { get; }

        /// <summary> The class a form is reflected out of. Its public instance fields ARE the
        /// parameters. </summary>
        Type ParametersType { get; }

        /// <summary> A fresh parameters instance with this generator's defaults. Constructed inside
        /// the SDK on purpose - the host never calls Activator over ParametersType, so the AOT
        /// compiler sees the instantiation and the stripper keeps it. </summary>
        object CreateDefaultParameters();

        /// <summary> Presentation facts reflection can't supply. Never null; GeneratorHints.Empty
        /// for a parameterless generator. </summary>
        GeneratorHints Hints { get; }

        /// <summary> What running this would add, without running it. Context is null for a Level
        /// generator (nothing exists yet to run against), so implementations must tolerate that. </summary>
        GeneratorCost Estimate(GeneratorContext context, object parameters);
    }
}
