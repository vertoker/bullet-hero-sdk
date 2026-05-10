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
    public class RuleIVector2MinAttribute : BaseRuleAttribute
    {
        // always include
        public float MinX { get; set; }
        public float MinY { get; set; }
        
        public RuleIVector2MinAttribute(float min)
        {
            MinX = min;
            MinY = min;
        }
        public RuleIVector2MinAttribute(float minX, float minY)
        {
            MinX = minX;
            MinY = minY;
        }

        protected override bool IsValidTypeInternal(object value) => value is IVector2;

        protected override bool IsValidInternal(object value, Level context)
        {
            if (value is not IVector2 vec) return false;

            switch (vec.GetModelType())
            {
                case VectorType.Value:
                {
                    var valueVec = (Vector2Value)value;
                    if (valueVec.X < MinX) return false;
                    if (valueVec.Y < MinY) return false;
                    return true;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector2Rect)value;
                    if (randomRect.MinX < MinX) return false;
                    if (randomRect.MinY < MinY) return false;
                    return true;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector2RectStep)value;
                    if (randomRectStep.MinX < MinX) return false;
                    if (randomRectStep.MinY < MinY) return false;
                    return true;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector2Circle)value;
                    if (randomCircle.X - randomCircle.Radius < MinX) return false;
                    if (randomCircle.Y - randomCircle.Radius < MinY) return false;
                    return true;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected override void FixInternal(object target, PropertyInfo property, Level context)
        {
            var value = property.GetValue(target);
            if (value is not IVector2 vec) return;

            switch (vec.GetModelType())
            {
                case VectorType.Value:
                {
                    var valueVec = (Vector2Value)value;
                    if (valueVec.X < MinX) valueVec.X = MathUtils.Max(valueVec.X, MinX);
                    if (valueVec.Y < MinY) valueVec.Y = MathUtils.Max(valueVec.Y, MinY);
                    break;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector2Rect)value;
                    if (randomRect.MinX < MinX) randomRect.MinX = MinX;
                    if (randomRect.MinY < MinY) randomRect.MinY = MinY;
                    break;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector2RectStep)value;
                    if (randomRectStep.MinX < MinX) randomRectStep.MinX = MinX;
                    if (randomRectStep.MinY < MinY) randomRectStep.MinY = MinY;
                    break;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector2Circle)value;
                    if (randomCircle.X - randomCircle.Radius < MinX) randomCircle.X = MinX + randomCircle.Radius;
                    if (randomCircle.Y - randomCircle.Radius < MinY) randomCircle.Y = MinY + randomCircle.Radius;
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}