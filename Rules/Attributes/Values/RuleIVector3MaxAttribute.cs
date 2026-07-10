using System;
using System.Reflection;
using BH.SDK.Models.Enum.Values;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Utils;

namespace BH.SDK.Rules.Attributes
{
    [AttributeUsage(PropertyTarget)]
    public class RuleIVector3MaxAttribute : BaseRuleAttribute
    {
        // always include
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float MaxZ { get; set; }
        
        public RuleIVector3MaxAttribute(float max)
        {
            MaxX = max;
            MaxY = max;
            MaxZ = max;
        }
        public RuleIVector3MaxAttribute(float maxX, float maxY, float maxZ)
        {
            MaxX = maxX;
            MaxY = maxY;
            MaxZ = maxZ;
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
                    if (valueVec.X > MaxX) return false;
                    if (valueVec.Y > MaxY) return false;
                    if (valueVec.Z > MaxZ) return false;
                    return true;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector3Rect)value;
                    if (randomRect.MaxX > MaxX) return false;
                    if (randomRect.MaxY > MaxY) return false;
                    if (randomRect.MaxZ > MaxZ) return false;
                    return true;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector3RectStep)value;
                    if (randomRectStep.MaxX > MaxX) return false;
                    if (randomRectStep.MaxY > MaxY) return false;
                    if (randomRectStep.MaxZ > MaxZ) return false;
                    return true;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector3Circle)value;
                    if (randomCircle.X + randomCircle.Radius > MaxX) return false;
                    if (randomCircle.Y + randomCircle.Radius > MaxY) return false;
                    if (randomCircle.Z + randomCircle.Radius > MaxZ) return false;
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
                    if (valueVec.X > MaxX) valueVec.X = BHSDKMath.Min(valueVec.X, MaxX);
                    if (valueVec.Y > MaxY) valueVec.Y = BHSDKMath.Min(valueVec.Y, MaxY);
                    if (valueVec.Z > MaxZ) valueVec.Z = BHSDKMath.Min(valueVec.Y, MaxZ);
                    break;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector3Rect)value;
                    if (randomRect.MaxX > MaxX) randomRect.MaxX = MaxX;
                    if (randomRect.MaxY > MaxY) randomRect.MaxY = MaxY;
                    if (randomRect.MaxZ > MaxZ) randomRect.MaxZ = MaxZ;
                    break;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector3RectStep)value;
                    if (randomRectStep.MaxX > MaxX) randomRectStep.MaxX = MaxX;
                    if (randomRectStep.MaxY > MaxY) randomRectStep.MaxY = MaxY;
                    if (randomRectStep.MaxZ > MaxZ) randomRectStep.MaxZ = MaxZ;
                    break;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector3Circle)value;
                    if (randomCircle.X + randomCircle.Radius > MaxX) randomCircle.X = MaxX - randomCircle.Radius;
                    if (randomCircle.Y + randomCircle.Radius > MaxY) randomCircle.Y = MaxY - randomCircle.Radius;
                    if (randomCircle.Z + randomCircle.Radius > MaxZ) randomCircle.Z = MaxZ - randomCircle.Radius;
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}