using System;
using BHSDK.Models.Enum.Effects;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace BHSDK.Models
{
    public static class EffectStatic
    {
        // -----------------------------------------------------------------------------
        // Defaults
        // -----------------------------------------------------------------------------
        
        public static readonly bool Core_ParticleColliderDefault = false;
        public static readonly bool Core_HasStopLocalFrameDefault = false;
        public static readonly int Core_StopLocalFrameDefault = 10;
        
        public static readonly bool Core_LoopDefault = true;
        public static readonly uint Core_ParticleCountDefault = 10;
        public static readonly Vector2 Core_LifetimeBoundsDefault = new(3f, 3f);
        public static readonly Vector2 Core_PivotDefault = new(0f, 0f);
        public static readonly Vector2 Core_GravityConstraintDefault = new(0f, -9.81f);
        public static readonly int Core_ParticleTextureIdDefault = 1;
        public static readonly int Core_ParticleTextureIndexDefault = 1;
        
        public static readonly byte Shape_TypeDefault = 0;
        public static readonly float Shape_CircleRadiusDefault = 1f;
        public static readonly float Shape_ArcDefault = Mathf.PI * 2f;
        public static readonly float Shape_CircleThicknessDefault = 1f;
        public static readonly Vector2 Shape_LineStartDefault = new(0f, 0f);
        public static readonly Vector2 Shape_LineEndDefault = new(1f, 0f);
        public static readonly float Shape_ConeBaseRadiusDefault = 1f;
        public static readonly float Shape_ConeTopRadiusDefault = 0.4f;
        public static readonly float Shape_ConeHeightDefault = 1f;
        public static readonly float Shape_TorusRadiusMinorDefault = 0.4f;
        public static readonly float Shape_TorusRadiusMajorDefault = 1f;
        public static readonly byte ShapeSpread_TypeDefault = 0;
        public static readonly float ShapeSpread_SpreadDefault = 0f;
        public static readonly float ShapeSpread_SpeedDefault = 1f;
        
        public static readonly float Forces_StartGravityMinDefault = 0f;
        public static readonly float Forces_StartGravityMaxDefault = 0f;
        public static readonly Vector2 Forces_StartVelocityMinDefault = new(0f, 0f);
        public static readonly Vector2 Forces_StartVelocityMaxDefault = new(0f, 0f);
        public static readonly float Forces_StartAngularVelocityMinDefault = 0f;
        public static readonly float Forces_StartAngularVelocityMaxDefault = 0f;
        public static readonly Vector3 Forces_OrbitalVelocityDefault = new(0f, 0f, 0f);
        public static readonly Vector2 Forces_LinearVelocityDefault = new(0f, 0f);
        public static readonly Vector3 Forces_OrbitalCenterOffsetDefault = new(0f, 0f, 0f);
        public static readonly float Forces_VelocitySpeedDefault = 1f;
        public static readonly Vector2 Forces_LinearForceDefault = new(0f, 0f);
        
        public static readonly Color Color_ADefault = Color.red;
        public static readonly Color Color_BDefault = Color.white;
        public static readonly byte Color_TypeDefault = 0;
        public static readonly Vector2 Color_BySpeedRangeDefault = new(1.3f, 2f);
        
        public static readonly Vector2 Scale_ADefault = new(1f, 1f);
        public static readonly Vector2 Scale_BDefault = new(1f, 1f);
        public static readonly byte Scale_TypeDefault = 0;
        public static readonly Vector2 Scale_BySpeedRangeDefault = new(0f, 1f);
        
        public static readonly float Angle_ADefault = 0f;
        public static readonly float Angle_BDefault = 0f;
        public static readonly byte Angle_TypeDefault = 0;
        public static readonly Vector2 Angle_BySpeedRangeDefault = new(0f, 1f);

        public static AnimationCurve GetDefaultCurve()
        {
            var key0 = new Keyframe(0f, 0f);
            var key1 = new Keyframe(1f, 1f);
            var curve = new AnimationCurve(key0, key1)
            {
                postWrapMode = WrapMode.Default,
                preWrapMode = WrapMode.Default
            };
            return curve;
        }
        public static Gradient GetDefaultGradient()
        {
            var gradient = new Gradient();
            
            Span<GradientColorKey> colorKeys = stackalloc GradientColorKey[2];
            colorKeys[0] = new GradientColorKey(Color.white, 0f);
            colorKeys[1] = new GradientColorKey(Color.white, 1f);
            
            Span<GradientAlphaKey> alphaKeys = stackalloc GradientAlphaKey[2];
            alphaKeys[0] = new GradientAlphaKey(1f, 0f);
            alphaKeys[1] = new GradientAlphaKey(1f, 1f);
            
            gradient.SetKeys(colorKeys, alphaKeys);
            gradient.mode = GradientMode.PerceptualBlend;
            gradient.colorSpace = ColorSpace.Linear;
            
            return gradient;
        }
    }
}