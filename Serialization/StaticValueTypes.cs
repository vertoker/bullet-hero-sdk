using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Values;

namespace BHSDK.Serialization
{
    public static class StaticValueTypes
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
        public static Type GetVectorType(VectorType type)
        {
            return type switch
            {
                VectorType.Value => typeof(VectorValue),
                VectorType.RandomRect => typeof(VectorRect),
                VectorType.RandomRectStep => typeof(VectorRectStep),
                VectorType.RandomCircle => typeof(VectorCircle),
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
    }
}