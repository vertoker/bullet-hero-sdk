// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Values;
using BH.SDK.Utils;

namespace BH.SDK.Rules
{
    public static class EffectRules
    {
        public const bool HasStopLocalFrame_Default = false;
        public const int StopLocalFrame_Default = 10;

        public static class Core
        {
            public const bool Render_Default = true;
            public const bool Loop_Default = true;
            public const bool IsLocal_Default = true;
            
            public const uint ParticleCount_Min = 0;
            public const uint ParticleCount_Max = 32768;
            public const uint ParticleCount_Default = 10;
            
            public const float LifetimeBounds_Min = 0f;
            public const float LifetimeBounds_X_Default = 3f;
            public const float LifetimeBounds_Y_Default = 3f;
            
            // Alignment.CenterMiddleValue.Get();
            public const float Pivot_X_Default = 0.5f;
            public const float Pivot_Y_Default = 0.5f;
            
            public const float GravityConstraint_X_Default = 0f;
            public const float GravityConstraint_Y_Default = -9.81f;
            
            public const int TextureResourceId_Default = 1; // circle (atlas + uv)
            
            // circle uv, it's more simple to use it
            public const float TextureResourceUV_X_Default = 0.0625f; // tilling x
            public const float TextureResourceUV_Y_Default = 0.0625f; // tilling y
            public const float TextureResourceUV_Z_Default = 0.0625f; // offset x
            public const float TextureResourceUV_W_Default = 0.9375f; // offset y
        }
        public static class Forces
        {
            public const float StartGravityMin_Default = 0f;
            
            public const float StartGravityMax_Default = 0f;
            
            public const float StartVelocityMin_X_Default = 0f;
            public const float StartVelocityMin_Y_Default = 0f;
            
            public const float StartVelocityMax_X_Default = 0f;
            public const float StartVelocityMax_Y_Default = 0f;
            
            public const float StartAngularVelocityMin_Default = 0f;
            public const float StartAngularVelocityMax_Default = 0f;
            
            public const float OrbitalVelocity_X_Default = 0f;
            public const float OrbitalVelocity_Y_Default = 0f;
            public const float OrbitalVelocity_Z_Default = 0f;
            
            public const float LinearVelocity_X_Default = 0f;
            public const float LinearVelocity_Y_Default = 0f;
            
            public const float OrbitalCenterOffset_X_Default = 0f;
            public const float OrbitalCenterOffset_Y_Default = 0f;
            public const float OrbitalCenterOffset_Z_Default = 0f;
            
            public const float VelocitySpeed_Default = 1f;
            
            public const float LinearForce_X_Default = 0f;
            public const float LinearForce_Y_Default = 0f;
        }
        public static class Shape
        {
            public const byte Type_Default = 0;
            
            public const float CircleRadius_Min = 0f;
            public const float CircleRadius_Default = 1f;
            
            public const float Arc_Min = 0f;
            public const float Arc_Max = MathUtils.PI2;
            public const float Arc_Default = Arc_Max;
            
            public const float CircleThickness_Min = 0f;
            public const float CircleThickness_Max = 1f;
            public const float CircleThickness_Default = CircleThickness_Max;
            
            public const float LineStart_X_Default = 0f;
            public const float LineStart_Y_Default = 0f;
            
            public const float LineEnd_X_Default = 1f;
            public const float LineEnd_Y_Default = 0f;
            
            public const float BoxSize_Min = 0f;
            public const float BoxSize_X_Default = 1f;
            public const float BoxSize_Y_Default = 1f;
            
            public const float ConeBaseRadius_Min = 0f;
            public const float ConeBaseRadius_Default = 1f;
            
            public const float ConeTopRadius_Min = 0f;
            public const float ConeTopRadius_Default = 0.4f;
            
            public const float ConeHeight_Min = 0f;
            public const float ConeHeight_Default = 1f;
            
            public const float TorusRadiusMinor_Min = 0f;
            public const float TorusRadiusMinor_Default = 0.4f;
            
            public const float TorusRadiusMajor_Min = 0f;
            public const float TorusRadiusMajor_Default = 1f;
        }
        public static class ShapeSpread
        {
            public const byte Type_Default = 0;
            
            public const float Spread_Default = 0f;
            
            public const float Speed_Default = 1f;
        }
        public static class Color
        {
            public const byte Type_Default = 0;
            
            public const float A_R_Default = 1f;
            public const float A_G_Default = 0f;
            public const float A_B_Default = 0f;
            public const float A_A_Default = 1f;
            
            public const float B_R_Default = 1f;
            public const float B_G_Default = 1f;
            public const float B_B_Default = 1f;
            public const float B_A_Default = 1f;
            
            public const float BySpeedRange_X_Default = 1.3f;
            public const float BySpeedRange_Y_Default = 2f;
        }
        public static class Scale
        {
            public const byte Type_Default = 0;
            
            public const float A_X_Default = 1f;
            public const float A_Y_Default = 1f;
            
            public const float B_X_Default = 1f;
            public const float B_Y_Default = 1f;
            
            public const float BySpeedRange_X_Default = 0f;
            public const float BySpeedRange_Y_Default = 1f;
        }
        public static class Angle
        {
            public const byte Type_Default = 0;
            
            public const float A_Default = 0f;
            
            public const float B_Default = 0f;
            
            public const float BySpeedRange_X_Default = 0f;
            public const float BySpeedRange_Y_Default = 1f;
        }
        
        public static CurveValue GetCurve_Default()
        {
            var key0 = new CurveKeyframeValue(0f, 0f);
            var key1 = new CurveKeyframeValue(1f, 1f);
            var keys = new List<CurveKeyframeValue> { key0, key1 };
            var curve = new CurveValue(keys, CurveWrapMode.Default, CurveWrapMode.Default);
            return curve;
        }
        public static GradientValue GetGradient_Default()
        {
            var colorKeys = new List<GradientColorKeyValue>
            {
                new(ColorValue.white, 0f),
                new(ColorValue.white, 1f),
            };
            var alphaKeys = new List<GradientAlphaKeyValue>
            {
                new(1f, 0f),
                new(1f, 1f),
            };
            
            return new GradientValue(colorKeys, alphaKeys,
                GradientInterpolationMode.PerceptualBlend, GradientColorSpace.Linear);
        }
    }
}