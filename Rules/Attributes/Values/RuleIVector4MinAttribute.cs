using System;
using System.Reflection;
using BHSDK.Models;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Utils;

namespace BHSDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleIVector4MinAttribute : BaseRuleAttribute
    {
        // always include
        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MinZ { get; set; }
        public float MinW { get; set; }

        public RuleIVector4MinAttribute(float min)
        {
            MinX = min;
            MinY = min;
            MinZ = min;
            MinW = min;
        }
        public RuleIVector4MinAttribute(float minX, float minY, float minZ, float minW)
        {
            MinX = minX;
            MinY = minY;
            MinZ = minZ;
            MinW = minW;
        }

        protected override bool IsValidTypeInternal(object value) => value is IVector4;

        protected override bool IsValidInternal(object value, Level context)
        {
            if (value is not IVector4 vec) return false;

            switch (vec.GetModelType())
            {
                case VectorType.Value:
                {
                    var valueVec = (Vector4Value)value;
                    if (valueVec.X < MinX) return false;
                    if (valueVec.Y < MinY) return false;
                    if (valueVec.Z < MinZ) return false;
                    if (valueVec.W < MinW) return false;
                    return true;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector4Rect)value;
                    if (randomRect.MinX < MinX) return false;
                    if (randomRect.MinY < MinY) return false;
                    if (randomRect.MinZ < MinZ) return false;
                    if (randomRect.MinW < MinW) return false;
                    return true;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector4RectStep)value;
                    if (randomRectStep.MinX < MinX) return false;
                    if (randomRectStep.MinY < MinY) return false;
                    if (randomRectStep.MinZ < MinZ) return false;
                    if (randomRectStep.MinW < MinW) return false;
                    return true;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector4Circle)value;
                    if (randomCircle.X - randomCircle.Radius < MinX) return false;
                    if (randomCircle.Y - randomCircle.Radius < MinY) return false;
                    if (randomCircle.Z - randomCircle.Radius < MinZ) return false;
                    if (randomCircle.W - randomCircle.Radius < MinW) return false;
                    return true;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            var value = property.GetValue(target);
            if (value is not IVector4 vec) return;

            switch (vec.GetModelType())
            {
                case VectorType.Value:
                {
                    var valueVec = (Vector4Value)value;
                    if (valueVec.X < MinX) valueVec.X = MathStatic.Max(valueVec.X, MinX);
                    if (valueVec.Y < MinY) valueVec.Y = MathStatic.Max(valueVec.Y, MinY);
                    if (valueVec.Z < MinZ) valueVec.Z = MathStatic.Max(valueVec.Y, MinZ);
                    if (valueVec.W < MinW) valueVec.W = MathStatic.Max(valueVec.W, MinW);
                    break;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector4Rect)value;
                    if (randomRect.MinX < MinX) randomRect.MinX = MinX;
                    if (randomRect.MinY < MinY) randomRect.MinY = MinY;
                    if (randomRect.MinZ < MinZ) randomRect.MinZ = MinZ;
                    if (randomRect.MinW < MinW) randomRect.MinW = MinW;
                    break;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector4RectStep)value;
                    if (randomRectStep.MinX < MinX) randomRectStep.MinX = MinX;
                    if (randomRectStep.MinY < MinY) randomRectStep.MinY = MinY;
                    if (randomRectStep.MinZ < MinZ) randomRectStep.MinZ = MinZ;
                    if (randomRectStep.MinW < MinW) randomRectStep.MinW = MinW;
                    break;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector4Circle)value;
                    if (randomCircle.X - randomCircle.Radius < MinX) randomCircle.X = MinX + randomCircle.Radius;
                    if (randomCircle.Y - randomCircle.Radius < MinY) randomCircle.Y = MinY + randomCircle.Radius;
                    if (randomCircle.Z - randomCircle.Radius < MinZ) randomCircle.Z = MinZ + randomCircle.Radius;
                    if (randomCircle.W - randomCircle.Radius < MinW) randomCircle.W = MinW + randomCircle.Radius;
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}