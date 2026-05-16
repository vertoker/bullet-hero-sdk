using System;
using System.Reflection;
using BHSDK.Models;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Utils;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleIVector4MaxAttribute : BaseRuleAttribute
    {
        // always include
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float MaxZ { get; set; }
        public float MaxW { get; set; }
        
        public RuleIVector4MaxAttribute(float max)
        {
            MaxX = max;
            MaxY = max;
            MaxZ = max;
            MaxW = max;
        }
        public RuleIVector4MaxAttribute(float maxX, float maxY, float maxZ, float maxW)
        {
            MaxX = maxX;
            MaxY = maxY;
            MaxZ = maxZ;
            MaxW = maxW;
        }

        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(IVector4).IsAssignableFrom(property.PropertyType);

        protected override bool IsValidInternal(object value, object context)
        {
            if (value is not IVector4 vec) return false;

            switch (vec.GetModelType())
            {
                case VectorType.Value:
                {
                    var valueVec = (Vector4Value)value;
                    if (valueVec.X > MaxX) return false;
                    if (valueVec.Y > MaxY) return false;
                    if (valueVec.Z > MaxZ) return false;
                    if (valueVec.W > MaxW) return false;
                    return true;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector4Rect)value;
                    if (randomRect.MaxX > MaxX) return false;
                    if (randomRect.MaxY > MaxY) return false;
                    if (randomRect.MaxZ > MaxZ) return false;
                    if (randomRect.MaxW > MaxW) return false;
                    return true;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector4RectStep)value;
                    if (randomRectStep.MaxX > MaxX) return false;
                    if (randomRectStep.MaxY > MaxY) return false;
                    if (randomRectStep.MaxZ > MaxZ) return false;
                    if (randomRectStep.MaxW > MaxW) return false;
                    return true;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector4Circle)value;
                    if (randomCircle.X + randomCircle.Radius > MaxX) return false;
                    if (randomCircle.Y + randomCircle.Radius > MaxY) return false;
                    if (randomCircle.Z + randomCircle.Radius > MaxZ) return false;
                    if (randomCircle.W + randomCircle.Radius > MaxW) return false;
                    return true;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            var value = property.GetValue(target);
            if (value is not IVector4 vec) return;

            switch (vec.GetModelType())
            {
                case VectorType.Value:
                {
                    var valueVec = (Vector4Value)value;
                    if (valueVec.X > MaxX) valueVec.X = MathUtils.Min(valueVec.X, MaxX);
                    if (valueVec.Y > MaxY) valueVec.Y = MathUtils.Min(valueVec.Y, MaxY);
                    if (valueVec.Z > MaxZ) valueVec.Z = MathUtils.Min(valueVec.Y, MaxZ);
                    if (valueVec.W > MaxW) valueVec.W = MathUtils.Min(valueVec.W, MaxW);
                    break;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector4Rect)value;
                    if (randomRect.MaxX > MaxX) randomRect.MaxX = MaxX;
                    if (randomRect.MaxY > MaxY) randomRect.MaxY = MaxY;
                    if (randomRect.MaxZ > MaxZ) randomRect.MaxZ = MaxZ;
                    if (randomRect.MaxW > MaxW) randomRect.MaxW = MaxW;
                    break;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector4RectStep)value;
                    if (randomRectStep.MaxX > MaxX) randomRectStep.MaxX = MaxX;
                    if (randomRectStep.MaxY > MaxY) randomRectStep.MaxY = MaxY;
                    if (randomRectStep.MaxZ > MaxZ) randomRectStep.MaxZ = MaxZ;
                    if (randomRectStep.MaxW > MaxW) randomRectStep.MaxW = MaxW;
                    break;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector4Circle)value;
                    if (randomCircle.X + randomCircle.Radius > MaxX) randomCircle.X = MaxX - randomCircle.Radius;
                    if (randomCircle.Y + randomCircle.Radius > MaxY) randomCircle.Y = MaxY - randomCircle.Radius;
                    if (randomCircle.Z + randomCircle.Radius > MaxZ) randomCircle.Z = MaxZ - randomCircle.Radius;
                    if (randomCircle.W + randomCircle.Radius > MaxW) randomCircle.W = MaxW - randomCircle.Radius;
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}