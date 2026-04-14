using System;
using BHSDK.Models.Enum.Effects;
using UnityEngine;

namespace BHSDK.Models
{
    public static class EffectStatic
    {
        // -----------------------------------------------------------------------------
        // Defaults
        // -----------------------------------------------------------------------------
        
        public static readonly bool ParticleColliderDefault = false;
        public static readonly bool HasStopTimeDefault = false;
        public static readonly float StopTimeDefault = 10f;
        
        public static readonly bool LoopDefault = true;
        public static readonly uint ParticleCountDefault = 10;
        public static readonly Vector2 LifetimeBoundsDefault = new(3f, 3f);
        public static readonly Vector2 PivotDefault = new(0f, 0f);
        public static readonly Vector2 GravityConstraintDefault = new(0f, -9.81f);
        public static readonly int ParticleTextureIdDefault = 1;
        
        public static readonly EffectShapeType ShapeTypeDefault = EffectShapeType.Point;
        public static readonly float CircleRadiusDefault = 1f;
        public static readonly float ShapeArcDefault = Mathf.PI * 2f;
        public static readonly float CircleThicknessDefault = 1f;
        public static readonly Vector2 LineStartDefault = new(0f, 0f);
        public static readonly Vector2 LineEndDefault = new(1f, 0f);
        public static readonly float ConeBaseRadiusDefault = 1f;
        public static readonly float ConeTopRadiusDefault = 0.4f;
        public static readonly float ConeHeightDefault = 1f;
        public static readonly float TorusRadiusMinorDefault = 0.4f;
        public static readonly float TorusRadiusMajorDefault = 1f;
        public static readonly EffectShapeSpreadType ShapeSpreadTypeDefault = EffectShapeSpreadType.Random;
        public static readonly float ShapeSpreadDefault = 0f;
        public static readonly float ShapeSpreadSpeedDefault = 1f;
        
        public static readonly float StartGravityMinDefault = 0f;
        public static readonly float StartGravityMaxDefault = 0f;
        public static readonly Vector2 StartVelocityMinDefault = new(0f, 0f);
        public static readonly Vector2 StartVelocityMaxDefault = new(0f, 0f);
        public static readonly float StartAngularVelocityMinDefault = 0f;
        public static readonly float StartAngularVelocityMaxDefault = 0f;
        public static readonly Vector3 OrbitalVelocityDefault = new(0f, 0f, 0f);
        public static readonly Vector2 LinearVelocityDefault = new(0f, 0f);
        public static readonly Vector3 OrbitalCenterOffsetDefault = new(0f, 0f, 0f);
        public static readonly float VelocitySpeedDefault = 1f;
        public static readonly Vector2 LinearForceDefault = new(0f, 0f);
        
        public static readonly Vector2 CollisionPositionDefault = new(0f, 0f);
        public static readonly float CollisionRotationDefault = 0f;
        public static readonly Vector2 CollisionScaleDefault = new(1f, 1f);
        public static readonly bool HasCollisionDefault = false;
        
        public static readonly Color ColorADefault = Color.red;
        public static readonly Color ColorBDefault = Color.white;
        public static readonly EffectColorType ColorTypeDefault = EffectColorType.Value;
        public static readonly Vector2 ColorBySpeedRangeDefault = new(1.3f, 2f);
        
        public static readonly Vector2 ScaleADefault = new(1f, 1f);
        public static readonly Vector2 ScaleBDefault = new(1f, 1f);
        public static readonly EffectScaleType ScaleTypeDefault = EffectScaleType.Value;
        public static readonly Vector2 ScaleBySpeedRangeDefault = new(0f, 1f);
        
        public static readonly float AngleADefault = 0f;
        public static readonly float AngleBDefault = 0f;
        public static readonly EffectAngleType AngleTypeDefault = EffectAngleType.Value;
        public static readonly Vector2 AngleBySpeedRangeDefault = new(0f, 1f);

        public static AnimationCurve GetDefaultCurve()
        {
            var key0 = new Keyframe(0f, 0f);
            var key1 = new Keyframe(1f, 1f);
            return new AnimationCurve(key0, key1);
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
            return gradient;
        }
    }
}