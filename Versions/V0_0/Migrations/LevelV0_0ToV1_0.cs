using BH.SDK.Models;
using BH.SDK.Models.Audio;

namespace BH.SDK.Versions.V0_0.Migrations
{
    // ReSharper disable once InconsistentNaming
    public class LevelV0_0ToV1_0 : DataMigration<LevelV0_0, Level>
    {
        public override Level Migrate(LevelV0_0 from) => new(
            from.Settings, // already upgraded to current LevelSettings by VersionedEnvelopeConverter
            from.Game, // already upgraded to current GameLevel by VersionedEnvelopeConverter
            new AudioLevel(), // Audio wasn't an independently-versioned domain at v0.0 (see AudioLevelV0_0 - no fields, no [DataVersion]), nothing to carry over
            from.Resources); // already upgraded to current LevelResources by VersionedEnvelopeConverter
    }
}
