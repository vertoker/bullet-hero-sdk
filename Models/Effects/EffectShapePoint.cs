using System;
using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Rules.Attributes;

namespace BHSDK.Models.Effects
{
    [RuleContainer]
    public class EffectShapePoint : IEffectShape,
        ICopyable<EffectShapePoint>, IEquatable<EffectShapePoint>
    {
        // None, inherit TRS from Object
        
        public EffectShapeType GetModelType() => EffectShapeType.Point;
        
        public object Clone() => Copy();
        IEffectShape ICopyable<IEffectShape>.Copy() => new EffectShapePoint();
        public EffectShapePoint Copy() => new();

        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        public override int GetHashCode() => base.GetHashCode();
        public override bool Equals(object obj) => obj is EffectShapePoint value && Equals(value);
        
        public bool Equals(IEffectShape other) => other is EffectShapePoint value && Equals(value);
        public bool Equals(EffectShapePoint other) => other is not null && ReferenceEquals(this, other);
    }
}