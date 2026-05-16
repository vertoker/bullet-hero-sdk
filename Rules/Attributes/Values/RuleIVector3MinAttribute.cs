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
    public class RuleIVector3MinAttribute : BaseRuleAttribute
    {
        // always include
        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MinZ { get; set; }

        public RuleIVector3MinAttribute(float min)
        {
            MinX = min;
            MinY = min;
            MinZ = min;
        }
        public RuleIVector3MinAttribute(float minX, float minY, float minZ)
        {
            MinX = minX;
            MinY = minY;
            MinZ = minZ;
        }

        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(IVector3).IsAssignableFrom(property.PropertyType);

        protected override bool IsValidInternal(object value, object context)
        {
            if (value is not IVector3 vec) return false;

            switch (vec.GetModelType())
            {
                case VectorType.Value:
                {
                    var valueVec = (Vector3Value)value;
                    if (valueVec.X < MinX) return false;
                    if (valueVec.Y < MinY) return false;
                    if (valueVec.Z < MinZ) return false;
                    return true;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector3Rect)value;
                    if (randomRect.MinX < MinX) return false;
                    if (randomRect.MinY < MinY) return false;
                    if (randomRect.MinZ < MinZ) return false;
                    return true;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector3RectStep)value;
                    if (randomRectStep.MinX < MinX) return false;
                    if (randomRectStep.MinY < MinY) return false;
                    if (randomRectStep.MinZ < MinZ) return false;
                    return true;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector3Circle)value;
                    if (randomCircle.X - randomCircle.Radius < MinX) return false;
                    if (randomCircle.Y - randomCircle.Radius < MinY) return false;
                    if (randomCircle.Z - randomCircle.Radius < MinZ) return false;
                    return true;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            var value = property.GetValue(target);
            if (value is not IVector3 vec) return;

            switch (vec.GetModelType())
            {
                case VectorType.Value:
                {
                    var valueVec = (Vector3Value)value;
                    if (valueVec.X < MinX) valueVec.X = MathUtils.Max(valueVec.X, MinX);
                    if (valueVec.Y < MinY) valueVec.Y = MathUtils.Max(valueVec.Y, MinY);
                    if (valueVec.Z < MinZ) valueVec.Z = MathUtils.Max(valueVec.Y, MinZ);
                    break;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector3Rect)value;
                    if (randomRect.MinX < MinX) randomRect.MinX = MinX;
                    if (randomRect.MinY < MinY) randomRect.MinY = MinY;
                    if (randomRect.MinZ < MinZ) randomRect.MinZ = MinZ;
                    break;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector3RectStep)value;
                    if (randomRectStep.MinX < MinX) randomRectStep.MinX = MinX;
                    if (randomRectStep.MinY < MinY) randomRectStep.MinY = MinY;
                    if (randomRectStep.MinZ < MinZ) randomRectStep.MinZ = MinZ;
                    break;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector3Circle)value;
                    if (randomCircle.X - randomCircle.Radius < MinX) randomCircle.X = MinX + randomCircle.Radius;
                    if (randomCircle.Y - randomCircle.Radius < MinY) randomCircle.Y = MinY + randomCircle.Radius;
                    if (randomCircle.Z - randomCircle.Radius < MinZ) randomCircle.Z = MinZ + randomCircle.Radius;
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}