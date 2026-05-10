namespace BHSDK.Rules
{
    public static class ValueRules
    {
        public const int MaxGameString = 256;
        
        public const int MaxMarkerEvents = 1024;
        public const int MaxCheckpointEvents = 128;
        public const int MaxBackgroundEvents = 128;
        public const int MaxThemeEvents = 128;
        
        public const int MaxCameraPositions = 512;
        public const int MaxCameraRotations = 512;
        public const int MaxCameraZooms = 512;
        public const int MaxCameraShakes = 512;
        
        public const int MaxObjectPositions = 32;
        public const int MaxObjectRotations = 32;
        public const int MaxObjectScales = 32;
        
        public const int MaxAudioVolumes = 32;
        public const int MaxAudioStereoPans = 32;
        
        public const int MaxPostProcessingEvents = 512;
        
        public const int MaxPlayerEvents = 512;
        
        // public const int MaxObjects = 100000; // no limit for objects count
        
        public const int MaxEditorName = 512;
        public const int MaxEditorDescription = 4096;
        
        public const int ThemeCount = 64;
        public const int MaxThemes = 32;
        public const int MaxPrefabs = 64;
        
        public const float MinColor = 0f;
        public const float MaxColor = 1f;
        // approximate size for min/max coordinates, because this allows
        // to calculate collision detection with at least 3 digits precision
        // S_max = (0.5·10⁻ᵈ) / (2ε) = 10⁻ᵈ / (4ε) = 2²¹ · 10⁻ᵈ = 2 097 152 · 10⁻ᵈ
        // d = 2, S_max = 20971.52, make this 10k on each side for more beauty.
        public const float MinCoord = -10000f;
        public const float MaxCoord = 10000f;
        // 100^2 = 10000, apply to coord rules
        public const float MinSca = -100f;
        public const float MaxSca = 100f;
        public const float MinZoom = 0f;
        public const float MaxZoom = 100f;
        public const int MinLayer = -1000;
        public const int MaxLayer = 1000;
    }
}