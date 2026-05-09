using System;
using System.Collections.Generic;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Values;

// ReSharper disable InconsistentNaming

namespace BHSDK.Utils
{
    public static class EffectStatic
    {
        // -----------------------------------------------------------------------------
        // Defaults
        // -----------------------------------------------------------------------------
        
        public const bool  Core_HasStopLocalFrame_Default = false;
        public const int   Core_StopLocalFrame_Default = 10;

        public const bool  Core_Loop_Default = true;
        public const uint  Core_ParticleCount_Default = 10;
        public const float Core_LifetimeBounds_X_Default = 3f;
        public const float Core_LifetimeBounds_Y_Default = 3f;
        public const float Core_Pivot_X_Default = 0.5f; // Alignment.MiddleCenterValue.Get();
        public const float Core_Pivot_Y_Default = 0.5f; // Alignment.MiddleCenterValue.Get();
        public const float Core_GravityConstraint_X_Default = 0f;
        public const float Core_GravityConstraint_Y_Default = -9.81f;
        public const int   Core_TextureResourceId_Default = 1;
        
        public const byte  Shape_Type_Default = 0;
        public const float Shape_CircleRadius_Default = 1f;
        public const float Shape_Arc_Default = MathStatic.PI2;
        public const float Shape_CircleThickness_Default = 1f;
        public const float Shape_LineStart_X_Default = 0f;
        public const float Shape_LineStart_Y_Default = 0f;
        public const float Shape_LineEnd_X_Default = 1f;
        public const float Shape_LineEnd_Y_Default = 0f;
        public const float Shape_BoxSize_X_Default = 1f;
        public const float Shape_BoxSize_Y_Default = 1f;
        public const float Shape_ConeBaseRadius_Default = 1f;
        public const float Shape_ConeTopRadius_Default = 0.4f;
        public const float Shape_ConeHeight_Default = 1f;
        public const float Shape_TorusRadiusMinor_Default = 0.4f;
        public const float Shape_TorusRadiusMajor_Default = 1f;
        public const byte  ShapeSpread_Type_Default = 0;
        public const float ShapeSpread_Spread_Default = 0f;
        public const float ShapeSpread_Speed_Default = 1f;
        
        public const float Forces_StartGravityMin_Default = 0f;
        public const float Forces_StartGravityMax_Default = 0f;
        public const float Forces_StartVelocityMin_X_Default = 0f;
        public const float Forces_StartVelocityMin_Y_Default = 0f;
        public const float Forces_StartVelocityMax_X_Default = 0f;
        public const float Forces_StartVelocityMax_Y_Default = 0f;
        public const float Forces_StartAngularVelocityMin_Default = 0f;
        public const float Forces_StartAngularVelocityMax_Default = 0f;
        public const float Forces_OrbitalVelocity_X_Default = 0f;
        public const float Forces_OrbitalVelocity_Y_Default = 0f;
        public const float Forces_OrbitalVelocity_Z_Default = 0f;
        public const float Forces_LinearVelocity_X_Default = 0f;
        public const float Forces_LinearVelocity_Y_Default = 0f;
        public const float Forces_OrbitalCenterOffset_X_Default = 0f;
        public const float Forces_OrbitalCenterOffset_Y_Default = 0f;
        public const float Forces_OrbitalCenterOffset_Z_Default = 0f;
        public const float Forces_VelocitySpeed_Default = 1f;
        public const float Forces_LinearForce_X_Default = 0f;
        public const float Forces_LinearForce_Y_Default = 0f;
        
        public const float Color_A_R_Default = 1f;
        public const float Color_A_G_Default = 0f;
        public const float Color_A_B_Default = 0f;
        public const float Color_A_A_Default = 1f;
        public const float Color_B_R_Default = 1f;
        public const float Color_B_G_Default = 1f;
        public const float Color_B_B_Default = 1f;
        public const float Color_B_A_Default = 1f;
        public const byte  Color_Type_Default = 0;
        public const float Color_BySpeedRange_X_Default = 1.3f;
        public const float Color_BySpeedRange_Y_Default = 2f;
        
        public const float Scale_A_X_Default = 1f;
        public const float Scale_A_Y_Default = 1f;
        public const float Scale_B_X_Default = 1f;
        public const float Scale_B_Y_Default = 1f;
        public const byte  Scale_Type_Default = 0;
        public const float Scale_BySpeedRange_X_Default = 0f;
        public const float Scale_BySpeedRange_Y_Default = 1f;
        
        public const float Angle_A_Default = 0f;
        public const float Angle_B_Default = 0f;
        public const byte  Angle_Type_Default = 0;
        public const float Angle_BySpeedRange_X_Default = 0f;
        public const float Angle_BySpeedRange_Y_Default = 1f;
        
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