using System;
using BH.SDK.Models.Enum.Effects;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Effects;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Effects
{
    [RuleContainer]
    public class EffectShapeCone : IEffectShape, IModel<EffectShapeCone>
    {
        [RuleNotNull, RuleIFloatMin(EffectRules.Shape.ConeTopRadius_Min)]
        [JsonProperty(Names.TopRadius)]
        public IFloat TopRadius { get; set; }
        
        [RuleNotNull, RuleIFloatMin(EffectRules.Shape.ConeBaseRadius_Min)]
        [JsonProperty(Names.BaseRadius)]
        public IFloat BaseRadius { get; set; }
        
        [RuleNotNull, RuleIFloatInRange(EffectRules.Shape.Arc_Min, EffectRules.Shape.Arc_Max)]
        [JsonProperty(Names.Arc)]
        public IFloat Arc { get; set; }
        
        [RuleNotNull, RuleIFloatMin(EffectRules.Shape.ConeHeight_Min)]
        [JsonProperty(Names.Height)]
        public IFloat Height { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Spread)]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Cone;
        
        public EffectShapeCone()
        {
            TopRadius = new FloatValue(EffectRules.Shape.ConeTopRadius_Default);
            BaseRadius = new FloatValue(EffectRules.Shape.ConeBaseRadius_Default);
            Arc = new FloatValue(EffectRules.Shape.Arc_Default);
            Height = new FloatValue(EffectRules.Shape.ConeHeight_Default);
            Spread = new EffectShapeSpreadRandom();
        }
        public EffectShapeCone(IFloat topRadius, IFloat baseRadius, IFloat arc, IFloat height, IEffectShapeSpread spread)
        {
            TopRadius = topRadius;
            BaseRadius = baseRadius;
            Arc = arc;
            Height = height;
            Spread = spread;
        }
        public void Reset()
        {
            TopRadius = new FloatValue(EffectRules.Shape.ConeTopRadius_Default);
            BaseRadius = new FloatValue(EffectRules.Shape.ConeBaseRadius_Default);
            Arc = new FloatValue(EffectRules.Shape.Arc_Default);
            Height = new FloatValue(EffectRules.Shape.ConeHeight_Default);
            Spread = new EffectShapeSpreadRandom();
        }

        public object Clone() => Copy();
        IEffectShape ICopyable<IEffectShape>.Copy() => new EffectShapeCone(TopRadius.Copy(), BaseRadius.Copy(), Arc.Copy(), Height.Copy(), Spread.Copy());
        public EffectShapeCone Copy() => new(TopRadius.Copy(), BaseRadius.Copy(), Arc.Copy(), Height.Copy(), Spread.Copy());

        public override bool Equals(object obj) => obj is EffectShapeCone value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(TopRadius, BaseRadius, Arc, Height, Spread);
        
        public bool Equals(IEffectShape other) => other is EffectShapeCone value && Equals(value);
        public bool Equals(EffectShapeCone other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = TopRadius.Equals(other.TopRadius)
                         && BaseRadius.Equals(other.BaseRadius)
                         && Arc.Equals(other.Arc)
                         && Height.Equals(other.Height)
                         && Spread.Equals(other.Spread);
            return result;
        }
    }
}