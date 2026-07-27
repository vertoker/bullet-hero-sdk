namespace BH.SDK.Versions
{
    // Domain name constants used by [DataVersion] on aggregate roots and by anything in
    // Serialization that needs to key per-domain behaviour (e.g. excluded converters) without
    // Models referencing Serialization types directly. See VERSION-UPDATE.md.
    public static class DataDomains
    {
        // Core
        public const string Level = nameof(Models.Level);
        public const string LevelMeta = nameof(Models.LevelMeta);
        public const string UserSettings = nameof(Models.UserSettings);
        public const string Prefab = nameof(Models.Objects.Prefab);
        public const string EffectObject = nameof(Models.Objects.EffectObject);
        public const string Theme = nameof(Models.Values.Theme);
        
        // Level
        public const string LevelSettings = nameof(Models.SettingGroups.LevelSettings);
        public const string GameLevel = nameof(Models.Game.GameLevel);
        public const string AudioLevel = nameof(Models.Audio.AudioLevel);
        public const string LevelResources = nameof(Models.Resources.LevelResources);
        
        // GameLevel
        public const string GameEvents = nameof(Models.Game.GameEvents);
        public const string CameraEvents = nameof(Models.Game.CameraEvents);
        public const string PostProcessingEvents = nameof(Models.Game.PostProcessingEvents);
        public const string PlayerEvents = nameof(Models.Game.PlayerEvents);
    }
}
