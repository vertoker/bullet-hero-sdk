using System;
using BHSDK.Models.Effects;
using BHSDK.Models.Enum;
using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Values;

namespace BHSDK.Serialization.Converters
{
    public static class ConverterStatics
    {
        public const string TypePropertyName = "t";
        public const string ValuePropertyName = "v";
        
        public static Type GetIntType(IntType type)
        {
            return type switch
            {
                IntType.Value => typeof(IntValue),
                IntType.RandomMinMax => typeof(IntMinMax),
                IntType.RandomMinMaxStep => typeof(IntMinMaxStep),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        public static Type GetFloatType(FloatType type)
        {
            return type switch
            {
                FloatType.Value => typeof(FloatValue),
                FloatType.RandomMinMax => typeof(FloatMinMax),
                FloatType.RandomMinMaxStep => typeof(FloatMinMaxStep),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        
        public static Type GetVector2Type(VectorType type)
        {
            return type switch
            {
                VectorType.Value => typeof(Vector2Value),
                VectorType.RandomRect => typeof(Vector2Rect),
                VectorType.RandomRectStep => typeof(Vector2RectStep),
                VectorType.RandomCircle => typeof(Vector2Circle),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        public static Type GetVector3Type(VectorType type)
        {
            return type switch
            {
                VectorType.Value => typeof(Vector3Value),
                VectorType.RandomRect => typeof(Vector3Rect),
                VectorType.RandomRectStep => typeof(Vector3RectStep),
                VectorType.RandomCircle => typeof(Vector3Circle),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        public static Type GetVector4Type(VectorType type)
        {
            return type switch
            {
                VectorType.Value => typeof(Vector4Value),
                VectorType.RandomRect => typeof(Vector4Rect),
                VectorType.RandomRectStep => typeof(Vector4RectStep),
                VectorType.RandomCircle => typeof(Vector4Circle),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        
        public static Type GetColorType(ColorType type)
        {
            return type switch
            {
                ColorType.Value => typeof(ColorValue),
                ColorType.RandomMinMax => typeof(ColorMinMax),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        public static Type GetScreenLimitType(ScreenLimitType type)
        {
            return type switch
            {
                ScreenLimitType.None => typeof(ScreenLimitNone),
                ScreenLimitType.Fixed => typeof(ScreenLimitFixed),
                ScreenLimitType.Bounds => typeof(ScreenLimitBounds),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        
        public static Type GetEffectShapeType(EffectShapeType type)
        {
            return type switch
            {
                EffectShapeType.Point => typeof(EffectShapePoint),
                EffectShapeType.Circle => typeof(EffectShapeCircle),
                EffectShapeType.Rectangle => typeof(EffectShapeRectangle),
                EffectShapeType.Line => typeof(EffectShapeLine),
                EffectShapeType.Cone => typeof(EffectShapeCone),
                EffectShapeType.Torus => typeof(EffectShapeTorus),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        public static Type GetEffectAngleType(EffectAngleType type)
        {
            return type switch
            {
                EffectAngleType.Value => typeof(EffectAngleValue),
                EffectAngleType.CurvesOverLife => typeof(EffectAngleCurvesOverLife),
                EffectAngleType.CurvesBySpeed => typeof(EffectAngleCurvesBySpeed),
                EffectAngleType.RandomUniform => typeof(EffectAngleRandomUniform),
                EffectAngleType.RandomPerComponent => typeof(EffectAngleRandomPerComponent),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        public static Type GetEffectScaleType(EffectScaleType type)
        {
            return type switch
            {
                EffectScaleType.Value => typeof(EffectScaleValue),
                EffectScaleType.CurvesOverLife => typeof(EffectScaleCurvesOverLife),
                EffectScaleType.CurvesBySpeed => typeof(EffectScaleCurvesBySpeed),
                EffectScaleType.RandomUniform => typeof(EffectScaleRandomUniform),
                EffectScaleType.RandomPerComponent => typeof(EffectScaleRandomPerComponent),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        public static Type GetEffectColorType(EffectColorType type)
        {
            return type switch
            {
                EffectColorType.Value => typeof(EffectColorValue),
                EffectColorType.GradientOverLife => typeof(EffectColorGradientOverLife),
                EffectColorType.GradientBySpeed => typeof(EffectColorGradientBySpeed),
                EffectColorType.RandomUniform => typeof(EffectColorRandomUniform),
                EffectColorType.RandomPerComponent => typeof(EffectColorRandomPerComponent),
                EffectColorType.GradientRandom => typeof(EffectColorGradientRandom),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        public static Type GetEffectShapeSpreadType(EffectShapeSpreadType type)
        {
            return type switch
            {
                EffectShapeSpreadType.Random => typeof(EffectShapeSpreadRandom),
                EffectShapeSpreadType.Loop => typeof(EffectShapeSpreadLoop),
                EffectShapeSpreadType.PingPong => typeof(EffectShapeSpreadPingPong),
                EffectShapeSpreadType.Sine => typeof(EffectShapeSpreadSine),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}