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
    public class EffectShapeCircle : IEffectShape, IModel<EffectShapeCircle>
    {
        [RuleNotNull, RuleIFloatMin(EffectRules.Shape.CircleRadius_Min)]
        [JsonProperty(Names.Radius)]
        public IFloat Radius { get; set; }
        
        [RuleNotNull, RuleIFloatInRange(EffectRules.Shape.CircleThickness_Min, EffectRules.Shape.CircleThickness_Max)]
        [JsonProperty(Names.Thickness)]
        public IFloat Thickness { get; set; }
        
        [RuleNotNull, RuleIFloatInRange(EffectRules.Shape.Arc_Min, EffectRules.Shape.Arc_Max)]
        [JsonProperty(Names.Arc)]
        public IFloat Arc { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Spread)]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Circle;

        public EffectShapeCircle()
        {
            Radius = new FloatValue(EffectRules.Shape.CircleRadius_Default);
            Thickness = new FloatValue(EffectRules.Shape.CircleThickness_Default);
            Arc = new FloatValue(EffectRules.Shape.Arc_Default);
            Spread = new EffectShapeSpreadRandom();
        }
        public EffectShapeCircle(IFloat radius, IFloat thickness, IFloat arc, IEffectShapeSpread spread)
        {
            Radius = radius;
            Thickness = thickness;
            Arc = arc;
            Spread = spread;
        }
        public void Reset()
        {
            Radius = new FloatValue(EffectRules.Shape.CircleRadius_Default);
            Thickness = new FloatValue(EffectRules.Shape.CircleThickness_Default);
            Arc = new FloatValue(EffectRules.Shape.Arc_Default);
            Spread = new EffectShapeSpreadRandom();
        }

        public object Clone() => Copy();
        IEffectShape ICopyable<IEffectShape>.Copy() => new EffectShapeCircle(Radius.Copy(), Thickness.Copy(), Arc.Copy(), Spread.Copy());
        public EffectShapeCircle Copy() => new(Radius.Copy(), Thickness.Copy(), Arc.Copy(), Spread.Copy());

        public override bool Equals(object obj) => obj is EffectShapeCircle value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Radius, Thickness, Arc, Spread);
        
        public bool Equals(IEffectShape other) => other is EffectShapeCircle value && Equals(value);
        public bool Equals(EffectShapeCircle other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Radius.Equals(other.Radius)
                         && Thickness.Equals(other.Thickness)
                         && Arc.Equals(other.Arc)
                         && Spread.Equals(other.Spread);
            return result;
        }
    }
}