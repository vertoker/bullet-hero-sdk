using BH.SDK.Utils;

namespace BH.SDK.Generators.Utility
{
    // LevelSettings.LimitHints is the format's one advisory value: peak simultaneous objects per
    // type, written so a player can preallocate its per-frame buffers instead of growing them
    // mid-level. The editor already refreshes it on every save (EditorService.Save), so this is not
    // about correctness - it is about being able to SEE the number, on demand, while deciding
    // whether a section is too heavy. That is why it is a generator rather than a hidden step: the
    // estimate line reports what the level currently peaks at, before anything is written.
    //
    // The one generator that neither creates nor edits objects. It exists partly to prove the
    // contract stretches that far.

    /// <summary>
    /// Recomputes the level's advisory capacity hint from what the level currently contains.
    /// </summary>
    public class CapacityHintGenerator : BaseContentGenerator<CapacityHintGenerator.Parameters>
    {
        public override string NameKey => "gen_capacity_hint";

        public override GeneratorRequirements Requirements => GeneratorRequirements.LevelScope;

        public override GeneratorHints Hints => GeneratorHints.Empty;

        protected override void Generate(GeneratorContext context, Parameters parameters)
        {
            var level = LevelOf(context);
            if (level == null) return;

            // Written straight onto Settings rather than through the context: LimitHints is not an
            // object, a resource or a level-global track, so there is no journal entry shape for it.
            // The consequence is deliberate and bounded - undoing this run leaves the refreshed
            // hint in place, which is harmless because the value is advisory and is recomputed on
            // every save anyway.
            level.Settings.LimitHints = LevelCapacityUtils.GetPeakUsage(level);
        }

        protected override GeneratorCost EstimateTyped(GeneratorContext context, Parameters parameters)
            => GeneratorCost.Zero;

        /// <summary> Rebuilds the owning Level from the context's own parts. The context carries the
        /// pieces rather than the Level itself (a Prefab-scoped run has no level), and LevelScope
        /// guarantees Game/Audio are present here. </summary>
        private static Models.Level LevelOf(GeneratorContext context)
        {
            if (context?.Game == null || context.Audio == null) return null;
            return new Models.Level(context.Settings, context.Game, context.Audio, context.Resources);
        }

        /// <summary> No parameters - the level itself is the whole input. </summary>
        public class Parameters
        {
        }
    }
}
