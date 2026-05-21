namespace BH.SDK.Rules
{
    public static class PostProcessingRules
    {
        public static class Bloom
        {
            public const float IntensityMin = 0f;
            public const float IntensityMax = 10f;
            
            public const float ScatterMin = 0f;
            public const float ScatterMax = 1f;
        }
        public static class ChromaticAberration
        {
            // TODO remove limits from Inspector
            public const float IntensityMin = 0f;
            public const float IntensityMax = 1f;
        }
        public static class Vignette
        {
            public const float CenterMin = 0f;
            public const float CenterMax = 1f;
            
            // TODO remove limits from Inspector
            public const float IntensityMin = 0f;
            public const float IntensityMax = 1f;
            
            // TODO remove limits from Inspector
            public const float SmoothnessMin = 0.01f;
            public const float SmoothnessMax = 1f;
        }
        public static class LensDistortion
        {
            public const float IntensityMin = -1f;
            public const float IntensityMax = 1f;
            
            public const float MultiplierMin = 0f;
            public const float MultiplierMax = 1f;
            
            public const float CenterMin = 0f;
            public const float CenterMax = 1f;
            
            public const float ScaleMin = 0.01f;
            public const float ScaleMax = 5f;
        }
        public static class FilmGrain
        {
            // TODO remove limits from Inspector
            public const float IntensityMin = 0f;
            public const float IntensityMax = 1f;
        }
        public static class MotionBlur
        {
            public const float IntensityMin = 0f;
            public const float IntensityMax = 1f;
        }
        public static class ColorCurves
        {
            public const float HueVsHueMin = 0f;
            public const float HueVsHueMax = 1f;
            
            public const float SatVsSatMin = 0f;
            public const float SatVsSatMax = 1f;
        }
        public static class ShadowsMidtonesHighlights
        {
            public const float ShadowLimitMin = 0f;
            public const float ShadowLimitMax = 1f;
            
            public const float HighlightLimitMin = 0f;
            public const float HighlightLimitMax = 1f;
        }
        public static class WhiteBalance
        {
            public const float TemperatureMin = -100f;
            public const float TemperatureMax = 100f;
            
            public const float TintMin = -100f;
            public const float TintMax = 100f;
        }
        public static class AnalogGlitch
        {
            public const float ScanLineJitterMin = 0f;
            public const float ScanLineJitterMax = 1f;
            
            public const float VerticalJumpMin = 0f;
            public const float VerticalJumpMax = 1f;
            
            public const float HorizontalShakeMin = 0f;
            public const float HorizontalShakeMax = 1f;
            
            public const float ColorDriftMin = 0f;
            public const float ColorDriftMax = 1f;
        }
        public static class DigitalGlitch
        {
            public const float IntensityMin = 0f;
            public const float IntensityMax = 1f;
        }
    }
}