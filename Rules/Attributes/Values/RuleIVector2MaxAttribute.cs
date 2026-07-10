using System;
using System.Reflection;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Utils;

namespace BH.SDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleIVector2MaxAttribute : BaseRuleAttribute
    {
        // always include
        public float MaxX { get; set; }
        public float MaxY { get; set; }

        public RuleIVector2MaxAttribute(float max)
        {
            MaxX = max;
            MaxY = max;
        }
        public RuleIVector2MaxAttribute(float maxX, float maxY)
        {
            MaxX = maxX;
            MaxY = maxY;
        }

        protected override bool IsValidTypeInternal(PropertyInfo property)
            => typeof(IVector2).IsAssignableFrom(property.PropertyType);

        protected override bool IsValidInternal(object value, object context)
        {
            if (value is not IVector2 vec) return false;

            switch (vec.GetModelType())
            {
                case VectorType.Value:
                {
                    var valueVec = (Vector2Value)value;
                    if (valueVec.X > MaxX) return false;
                    if (valueVec.Y > MaxY) return false;
                    return true;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector2Rect)value;
                    if (randomRect.MaxX > MaxX) return false;
                    if (randomRect.MaxY > MaxY) return false;
                    return true;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector2RectStep)value;
                    if (randomRectStep.MaxX > MaxX) return false;
                    if (randomRectStep.MaxY > MaxY) return false;
                    return true;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector2Circle)value;
                    if (randomCircle.X + randomCircle.Radius > MaxX) return false;
                    if (randomCircle.Y + randomCircle.Radius > MaxY) return false;
                    return true;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }

        protected override void FixInternal(object target, PropertyInfo property, object context)
        {
            var value = property.GetValue(target);
            if (value is not IVector2 vec) return;

            switch (vec.GetModelType())
            {
                case VectorType.Value:
                {
                    var valueVec = (Vector2Value)value;
                    if (valueVec.X > MaxX) valueVec.X = BHSDKMath.Min(valueVec.X, MaxX);
                    if (valueVec.Y > MaxY) valueVec.Y = BHSDKMath.Min(valueVec.Y, MaxY);
                    break;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector2Rect)value;
                    if (randomRect.MaxX > MaxX) randomRect.MaxX = MaxX;
                    if (randomRect.MaxY > MaxY) randomRect.MaxY = MaxY;
                    break;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector2RectStep)value;
                    if (randomRectStep.MaxX > MaxX) randomRectStep.MaxX = MaxX;
                    if (randomRectStep.MaxY > MaxY) randomRectStep.MaxY = MaxY;
                    break;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector2Circle)value;
                    if (randomCircle.X + randomCircle.Radius > MaxX) randomCircle.X = MaxX - randomCircle.Radius;
                    if (randomCircle.Y + randomCircle.Radius > MaxY) randomCircle.Y = MaxY - randomCircle.Radius;
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}