using System;
using BH.SDK.Models.Enum.Effects;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Effects;
using BH.SDK.Rules.Attributes;

namespace BH.SDK.Models.Effects
{
    [RuleContainer]
    public class EffectShapeSpreadSine : IEffectShapeSpread, IModel<EffectShapeSpreadSine>
    {
        public EffectShapeSpreadType GetModelType() => EffectShapeSpreadType.Sine;
        
        public void Reset() { }
        
        public object Clone() => Copy();
        IEffectShapeSpread ICopyable<IEffectShapeSpread>.Copy() => new EffectShapeSpreadSine();
        public EffectShapeSpreadSine Copy() => new();

        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        public override int GetHashCode() => base.GetHashCode();
        public override bool Equals(object obj) => obj is EffectShapeSpreadSine value && Equals(value);
        
        public bool Equals(IEffectShapeSpread other) => other is EffectShapeSpreadSine value && Equals(value);
        public bool Equals(EffectShapeSpreadSine other) => other is not null;
    }
}