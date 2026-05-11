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
    public class RuleIVector4InRangeAttribute : BaseRuleAttribute
    {
        // always include
        public float MinX { get; set; }
        public float MaxX { get; set; }
        public float MinY { get; set; }
        public float MaxY { get; set; }
        public float MinZ { get; set; }
        public float MaxZ { get; set; }
        public float MinW { get; set; }
        public float MaxW { get; set; }
        
        public float DiffX => MaxX - MinX;
        public float DiffY => MaxY - MinY;
        public float DiffZ => MaxZ - MinZ;
        public float DiffW => MaxW - MinW;
        public float HalfDiffX => (MaxX - MinX) / 2f;
        public float HalfDiffY => (MaxY - MinY) / 2f;
        public float HalfDiffZ => (MaxZ - MinZ) / 2f;
        public float HalfDiffW => (MaxW - MinW) / 2f;

        public RuleIVector4InRangeAttribute(float min, float max)
        {
            MinX = min;
            MaxX = max;
            MinY = min;
            MaxY = max;
            MinZ = min;
            MaxZ = max;
            MinW = min;
            MaxW = max;
        }
        public RuleIVector4InRangeAttribute(float minX, float maxX, float minY, float maxY,
            float minZ, float maxZ, float minW, float maxW)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
            MinZ = minZ;
            MaxZ = maxZ;
            MinW = minW;
            MaxW = maxW;
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
                    if (valueVec.X < MinX || valueVec.X > MaxX) return false;
                    if (valueVec.Y < MinY || valueVec.Y > MaxY) return false;
                    if (valueVec.Z < MinZ || valueVec.Z > MaxZ) return false;
                    if (valueVec.W < MinW || valueVec.W > MaxW) return false;
                    return true;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector4Rect)value;
                    if (randomRect.MinX < MinX || randomRect.MaxX > MaxX) return false;
                    if (randomRect.MinY < MinY || randomRect.MaxY > MaxY) return false;
                    if (randomRect.MinZ < MinZ || randomRect.MaxZ > MaxZ) return false;
                    if (randomRect.MinW < MinW || randomRect.MaxW > MaxW) return false;
                    return true;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector4RectStep)value;
                    if (randomRectStep.MinX < MinX || randomRectStep.MaxX > MaxX) return false;
                    if (randomRectStep.MinY < MinY || randomRectStep.MaxY > MaxY) return false;
                    if (randomRectStep.MinZ < MinZ || randomRectStep.MaxZ > MaxZ) return false;
                    if (randomRectStep.MinW < MinW || randomRectStep.MaxW > MaxW) return false;
                    return true;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector4Circle)value;
                    if (randomCircle.Radius > HalfDiffX) return false;
                    if (randomCircle.Radius > HalfDiffY) return false;
                    if (randomCircle.Radius > HalfDiffZ) return false;
                    if (randomCircle.Radius > HalfDiffW) return false;
                    if (randomCircle.X - randomCircle.Radius < MinX) return false;
                    if (randomCircle.X + randomCircle.Radius > MaxX) return false;
                    if (randomCircle.Y - randomCircle.Radius < MinY) return false;
                    if (randomCircle.Y + randomCircle.Radius > MaxY) return false;
                    if (randomCircle.Z - randomCircle.Radius < MinZ) return false;
                    if (randomCircle.Z + randomCircle.Radius > MaxZ) return false;
                    if (randomCircle.W - randomCircle.Radius < MinW) return false;
                    if (randomCircle.W + randomCircle.Radius > MaxW) return false;
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
                    if (valueVec.X < MinX || valueVec.X > MaxX) valueVec.X = MathUtils.Clamp(valueVec.X, MinX, MaxX);
                    if (valueVec.Y < MinY || valueVec.Y > MaxY) valueVec.Y = MathUtils.Clamp(valueVec.Y, MinY, MaxY);
                    if (valueVec.Z < MinZ || valueVec.Z > MaxZ) valueVec.Z = MathUtils.Clamp(valueVec.Y, MinZ, MaxZ);
                    if (valueVec.W < MinW || valueVec.W > MaxW) valueVec.W = MathUtils.Clamp(valueVec.W, MinW, MaxW);
                    break;
                }
                case VectorType.RandomRect:
                {
                    var randomRect = (Vector4Rect)value;
                    if (randomRect.MinX < MinX) randomRect.MinX = MinX;
                    if (randomRect.MaxX > MaxX) randomRect.MaxX = MaxX;
                    if (randomRect.MinY < MinY) randomRect.MinY = MinY;
                    if (randomRect.MaxY > MaxY) randomRect.MaxY = MaxY;
                    if (randomRect.MinZ < MinZ) randomRect.MinZ = MinZ;
                    if (randomRect.MaxZ > MaxZ) randomRect.MaxZ = MaxZ;
                    if (randomRect.MinW < MinW) randomRect.MinW = MinW;
                    if (randomRect.MaxW > MaxW) randomRect.MaxW = MaxW;
                    break;
                }
                case VectorType.RandomRectStep:
                {
                    var randomRectStep = (Vector4RectStep)value;
                    if (randomRectStep.MinX < MinX) randomRectStep.MinX = MinX;
                    if (randomRectStep.MaxX > MaxX) randomRectStep.MaxX = MaxX;
                    if (randomRectStep.MinY < MinY) randomRectStep.MinY = MinY;
                    if (randomRectStep.MaxY > MaxY) randomRectStep.MaxY = MaxY;
                    if (randomRectStep.MinZ < MinZ) randomRectStep.MinZ = MinZ;
                    if (randomRectStep.MaxZ > MaxZ) randomRectStep.MaxZ = MaxZ;
                    if (randomRectStep.MinW < MinW) randomRectStep.MinW = MinW;
                    if (randomRectStep.MaxW > MaxW) randomRectStep.MaxW = MaxW;
                    break;
                }
                case VectorType.RandomCircle:
                {
                    var randomCircle = (Vector4Circle)value;
                    
                    if (randomCircle.Radius > HalfDiffX) randomCircle.Radius = HalfDiffX;
                    else if (randomCircle.Radius > HalfDiffY) randomCircle.Radius = HalfDiffY;
                    else if (randomCircle.Radius > HalfDiffZ) randomCircle.Radius = HalfDiffZ;
                    else if (randomCircle.Radius > HalfDiffW) randomCircle.Radius = HalfDiffW;
                    
                    if (randomCircle.X - randomCircle.Radius < MinX)      randomCircle.X = MinX + randomCircle.Radius;
                    else if (randomCircle.X + randomCircle.Radius > MaxX) randomCircle.X = MaxX - randomCircle.Radius;
                    if (randomCircle.Y - randomCircle.Radius < MinY)      randomCircle.Y = MinY + randomCircle.Radius;
                    else if (randomCircle.Y + randomCircle.Radius > MaxY) randomCircle.Y = MaxY - randomCircle.Radius;
                    if (randomCircle.Z - randomCircle.Radius < MinZ)      randomCircle.Z = MinZ + randomCircle.Radius;
                    else if (randomCircle.Z + randomCircle.Radius > MaxZ) randomCircle.Z = MaxZ - randomCircle.Radius;
                    if (randomCircle.W - randomCircle.Radius < MinW)      randomCircle.W = MinW + randomCircle.Radius;
                    else if (randomCircle.W + randomCircle.Radius > MaxW) randomCircle.W = MaxW - randomCircle.Radius;
                    
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}