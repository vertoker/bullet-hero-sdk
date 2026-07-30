namespace BH.SDK.Models
{
    public static class FileNames
    {
        public const string LevelDirectory = "levels";

        public const string SettingsFileName = "settings.json";

        // No fixed extension - level.json/level.bson and metadata.json/metadata.bson are chosen
        // per-level at creation time and resolved by which extension is present on disk at load
        // time (see PathUtils.FindDataFile), not stored as a field in Level/LevelMeta itself.
        public const string LevelFileBaseName = "level";
        public const string MetadataFileBaseName = "metadata";

        public const string LogoName = "logo";
        public const string LogoFileNamePng = "logo.png";
        public const string LogoFileNameJpg = "logo.jpg";

        // Device-wide (not per-level) shared library of reusable Themes/Effects/Colliders - see
        // PathUtils.GetThemesDirectoryInfo/GetEffectsDirectoryInfo/GetCollidersDirectoryInfo.
        public const string ResourcesDirectory = "resources";
        public const string ThemesDirectory = "themes";
        public const string EffectsDirectory = "effects";
        public const string CollidersDirectory = "colliders";
        
        public const string ReportsDirectory = "reports";
    }
}