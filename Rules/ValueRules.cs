namespace BH.SDK.Rules
{
    public static class ValueRules
    {
        public const int IntZero = 0;
        public const int IntOne = 1;
        public const float FloatZero = 0f;
        public const float FloatOne = 1f;
        
        // default value limits without type specification.
        // Choose it because Max * Max => close to int.MaxValue 
        public const int MinIntValue = -1_000_000;
        public const int MaxIntValue = 1_000_000;
        public const float MinFloatValue = -1_000_000f;
        public const float MaxFloatValue = 1_000_000f;
        
        // convert from logical layer to real z position
        public const float LayerCoefficient = -1f;
        // minimal allowed delta for no clipping editor object through each other
        public const float MinLayerDelta = 0.01f;
        
        public const int MinLayer = -1000;
        public const int MaxLayer = 1000;
        public const float MinLayerSelection = MaxLayer + MinLayerDelta;
        public const int MinLayerGizmos = 1500;

        // Not an authored limit and not validated by any rule - the camera has no Layer field. This
        // is the z range the camera itself is allowed to occupy at runtime, wide enough to sit
        // outside every authored layer and every editor overlay band above them.
        public const float MinCameraLayer = -2000f;
        public const float MaxCameraLayer = 2000f;

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
        public const float DefaultZoom = 10f;
        
        public const float DefaultUvX = 1f; // tilling x
        public const float DefaultUvY = 1f; // tilling y
        public const float DefaultUvZ = 0f; // offset x
        public const float DefaultUvW = 0f; // offset y
        
        public const int MinThemeIndex = 0;
        public const int MaxThemeIndex = 63;
        public const int ThemeCount = 64;
        
        // A collider needs at least one triangle to be a shape at all - an empty one is a collider
        // that silently never collides, which is worse than no collider (that is what a Null
        // ColliderId already means, explicitly).
        public const int MinColliderTriangles = 1;
        public const int MaxColliderTriangles = 64;

        // A curve needs two keys to define a segment and a gradient two stops to define a blend.
        // Below that there is nothing to interpolate between, and every consumer would have to
        // invent a fallback of its own.
        public const int MinCurveKeys = 2;
        public const int MaxCurveKeys = 4;
        public const int MinGradientKeys = 2;
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

        // BCP-47 tags: "en", "pt-BR", "zh-Hans-CN". Bounded and shaped, because the code is a lookup
        // key - an unbounded or malformed one just silently never matches a player's locale.
        public const int MaxLanguageCode = 16;
        public const string LanguageCodePattern = "^[A-Za-z]{2,8}(-[A-Za-z0-9]{1,8})*$";

        // Licence text is the one field in the whole format meant to hold a wall of prose (a full
        // MIT/CC licence body), so it gets its own generous cap instead of the description one.
        public const int MaxLicenseName = 256;
        public const int MaxLicenseText = 65_536;

        // A Modification's field path ("pos[0].v"). Depth is what makes a path long, and the model
        // tree is nowhere near deep enough to need more than this.
        public const int MaxModificationPath = 256;
    }
}