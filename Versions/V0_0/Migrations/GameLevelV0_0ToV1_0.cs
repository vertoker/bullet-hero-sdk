using System.Collections.Generic;
using BH.SDK.Models.Game;
using BH.SDK.Models.Objects;

namespace BH.SDK.Versions.V0_0.Migrations
{
    // ReSharper disable once InconsistentNaming
    public class GameLevelV0_0ToV1_0 : DataMigration<GameLevelV0_0, GameLevel>
    {
        public override GameLevel Migrate(GameLevelV0_0 from) => new(
            from.GameEvents, // already upgraded to current GameEvents by VersionedEnvelopeConverter
            new CameraEvents(), // didn't exist at v0.0
            new PostProcessingEvents(), // didn't exist at v0.0
            new PlayerEvents(), // didn't exist at v0.0
            from.Objects,
            new List<PrefabObject>()); // didn't exist at v0.0
    }
}
