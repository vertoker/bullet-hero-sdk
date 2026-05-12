using System;
using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Rules.Attributes;

namespace BHSDK.Models.Effects
{
    [RuleContainer]
    public class EffectShapeSpreadSine : IEffectShapeSpread,
        ICopyable<EffectShapeSpreadSine>, IEquatable<EffectShapeSpreadSine>
    {
        public EffectShapeSpreadType GetModelType() => EffectShapeSpreadType.Sine;
        
        public object Clone() => Copy();
        IEffectShapeSpread ICopyable<IEffectShapeSpread>.Copy() => new EffectShapeSpreadSine();
        public EffectShapeSpreadSine Copy() => new();

        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        public override int GetHashCode() => base.GetHashCode();
        public override bool Equals(object obj) => obj is EffectShapeSpreadSine value && Equals(value);
        
        public bool Equals(IEffectShapeSpread other) => other is EffectShapeSpreadSine value && Equals(value);
        public bool Equals(EffectShapeSpreadSine other) => other is not null && ReferenceEquals(this, other);
    }
}