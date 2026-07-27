using System.Collections.Generic;
using BH.SDK.Models.Game;
using BH.SDK.Models.Objects;
using BH.SDK.Models.Primitives;
using Newtonsoft.Json;

namespace BH.SDK.Versions.V0_0
{
    // ReSharper disable once InconsistentNaming

    [DataVersion(DataDomains.GameLevel, 0, 0)]
    public class GameLevelV0_0
    {
        // GameEvents is its own independently-versioned domain - must be typed as the CURRENT
        // GameEvents class, same reasoning as LevelV0_0.Settings/.Game/.Resources.
        [JsonProperty("test_game_events")]
        public GameEvents GameEvents { get; set; }

        // Objects holds per-instance polymorphic RectObject variants (ObjectConverter's job, not
        // domain versioning) - never independently versioned, same type on both sides.
        [JsonProperty("test_objects")]
        public Dictionary<ObjectId, RectObject> Objects { get; set; }
    }
}