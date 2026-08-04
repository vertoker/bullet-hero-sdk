namespace BH.SDK.Rules
{
    public static class LevelRules
    {
        public const int MaxMarkerEvents = 1024;
        public const int MaxCheckpointEvents = 128;
        public const int MaxBackgroundEvents = 128;
        public const int MaxThemeEvents = 128;
        
        public const int MaxPlayerKeys = 512;
        public const int MaxCameraKeys = 512;
        public const int MaxPostProcessingKeys = 512;
        public const int MaxObjectKeys = 32;
        public const int MaxAudioKeys = 32;
        
        // public const int MaxObjects = 100000; // no limit for objects count

        public const int MaxPrefabs = 64;

        // Bounds of LevelCapacityHint - purely a format-level sanity clamp, so a corrupted or
        // hostile file can't ask a player's device to preallocate gigabytes before the runtime even
        // looks at the number. The real ceiling is per-device and applied at runtime; the hint
        // itself is advisory and never trusted on its own.
        public const int MinCapacityHint = 0;
        public const int MaxCapacityHint = 1_000_000;
    }
}