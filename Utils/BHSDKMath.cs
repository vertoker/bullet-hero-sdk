using System;

namespace BH.SDK.Utils
{
    // Mixed functions from UnityEngine.Mathf and Unity.Collections.math
    public static class BHSDKMath
    {
        public const float PI = 3.1415927f;
        public const float PI2 = PI * 2f;
        
        public const float Epsilon = 1.1920928955078125e-7f;
        public const float ApproxEpsilon = Epsilon * 8f;
        
        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            return value <= max ? value : max;
        }
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            return value <= max ? value : max;
        }
        public static float Clamp01(float value)
        {
            if (value < 0.0) return 0.0f;
            return value <= 1.0 ? value : 1f;
        }
        
        public static float Min(float a, float b) => a >= b ? b : a;
        public static float Max(float a, float b) => a <= b ? b : a;
        public static float Abs(float f) => Math.Abs(f);
        
        public static bool Approximately(float a, float b)
        {
            return Abs(b - a) < Max(1E-06f * Max(Abs(a), Abs(b)), ApproxEpsilon);
        }
    }
}