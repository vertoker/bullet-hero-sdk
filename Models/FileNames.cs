namespace BH.SDK.Models
{
    /// <summary>
    /// Fixed names of everything the game reads from disk - level folders, save files, shared
    /// libraries. Centralized so a level folder authored by one build is readable by any other.
    /// </summary>
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

        // Device-wide (not per-level) shared library of reusable Themes/Effects/Shapes/Prefabs -
        // see PathUtils.GetThemesDirectoryInfo/GetEffectsDirectoryInfo/GetShapesDirectoryInfo/
        // GetPrefabsDirectoryInfo.
        public const string ResourcesDirectory = "resources";
        public const string ThemesDirectory = "themes";
        public const string EffectsDirectory = "effects";
        public const string ShapesDirectory = "shapes";
        public const string PrefabsDirectory = "prefabs";
        
        public const string ReportsDirectory = "reports";
    }
}