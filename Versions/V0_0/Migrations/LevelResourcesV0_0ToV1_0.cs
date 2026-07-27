using BH.SDK.Models.Resources;

namespace BH.SDK.Versions.V0_0.Migrations
{
    // ReSharper disable once InconsistentNaming
    public class LevelResourcesV0_0ToV1_0 : DataMigration<LevelResourcesV0_0, LevelResources>
    {
        // LevelResourcesV0_0.Resources was placeholder test data with no correspondence to the
        // current Textures/Fonts/Audios/CompositeShapes/Themes/Prefabs shape - nothing to map.
        public override LevelResources Migrate(LevelResourcesV0_0 from) => new();
    }
}
