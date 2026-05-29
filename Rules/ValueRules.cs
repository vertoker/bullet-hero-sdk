namespace BH.SDK.Rules
{
    public static class ValueRules
    {
        public const int ZeroInt = 0;
        public const float ZeroFloat = 0f;
        
        // default value limits without type specification.
        // Choose it because Max * Max => close to int.MaxValue 
        public const int MinIntValue = -1_000_000;
        public const int MaxIntValue = 1_000_000;
        public const float MinFloatValue = -1_000_000f;
        public const float MaxFloatValue = 1_000_000f;
        
        public const int MinLayer = -1000;
        public const int MaxLayer = 1000;
        public const int DefaultLayer = 0;
        
        public const float MinColor = 0f;
        public const float MaxColor = 1f;
        public const float DefaultColorR = 1f;
        public const float DefaultColorG = 1f;
        public const float DefaultColorB = 1f;
        public const float DefaultColorA = 1f;
        
        // approximate size for min/max coordinates, because this allows
        // to calculate collision detection with at least 3 digits precision
        // S_max = (0.5·10⁻ᵈ) / (2ε) = 10⁻ᵈ / (4ε) = 2²¹ · 10⁻ᵈ = 2 097 152 · 10⁻ᵈ
        // d = 2, S_max = 20971.52, make this 10k on each side for more beauty.
        public const float MinPos = -10000f;
        public const float MaxPos = 10000f;
        public const float DefaultPosX = 0f;
        public const float DefaultPosY = 0f;
        
        // 100^2 = 10000, apply to coord rules
        public const float MinSca = -100f;
        public const float MaxSca = 100f;
        public const float DefaultScaX = 1f;
        public const float DefaultScaY = 1f;
        
        // 100^2 = 10000, apply to coord rules
        public const float MinAlignment = -100f;
        public const float MaxAlignment = 100f;
        public const float DefaultAlignmentX = 0.5f;
        public const float DefaultAlignmentY = 0.5f;
        
        public const float MinZoom = 0f;
        public const float MaxZoom = 100f;
        public const float DefaultZoomX = 1f;
        public const float DefaultZoomY = 1f;
        
        public const int MinThemeIndex = 0;
        public const int MaxThemeIndex = 63;
        public const int ThemeCount = 64;
        
        public const int MaxColliderTriangles = 64;
        
        public const int MaxCurveKeys = 4;
        public const int MaxGradientKeys = 4;
        
        public const float MinCurveTime = 0f;
        public const float MaxCurveTime = 1f;
        public const float MinGradientTime = 0f;
        public const float MaxGradientTime = 1f;
        
        public const int MinAspectWidth = 1;
        public const int MinAspectHeight = 1;
        public const int MaxAspectWidth = 100;
        public const int MaxAspectHeight = 100;
        public const int DefaultAspectWidth = 16;
        public const int DefaultAspectHeight = 9;
        
        public const int MaxGameString = 256;
        public const string DefaultLanguageCode = "en";
        public const int MaxUrl = 512;
        public const int MaxEditorName = 512;
        public const int MaxEditorDescription = 4096;
    }
}