using BH.SDK.Models.Primitives;
using BH.SDK.Models.SettingGroups;

namespace BH.SDK.Versions.V0_0.Migrations
{
    // ReSharper disable once InconsistentNaming
    public class LevelSettingsV0_0ToV1_0 : DataMigration<LevelSettingsV0_0, LevelSettings>
    {
        public override LevelSettings Migrate(LevelSettingsV0_0 from) => new(
            from.Framerate,
            from.Framerate * 10, // FrameLength didn't exist at v0.0 - same derivation as LevelSettings' own default ctor
            ObjectId.MinLevelValue, // ObjectIdCounter didn't exist at v0.0
            AudioId.MinValue); // AudioIdCounter didn't exist at v0.0
    }
}
