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
    public class EffectShapeTorus : IEffectShape,
        ICopyable<EffectShapeTorus>, IEquatable<EffectShapeTorus>
    {
        [RuleNotNull, RuleIFloatMin(EffectRules.Shape.TorusRadiusMinor_Min)]
        [JsonProperty(Names.RadiusMinor)]
        public IFloat RadiusMinor { get; set; }
        
        [RuleNotNull, RuleIFloatMin(EffectRules.Shape.TorusRadiusMajor_Min)]
        [JsonProperty(Names.RadiusMajor)]
        public IFloat RadiusMajor { get; set; }
        
        [RuleNotNull, RuleIFloatInRange(EffectRules.Shape.Arc_Min, EffectRules.Shape.Arc_Max)]
        [JsonProperty(Names.Arc)]
        public IFloat Arc { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Spread)]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Torus;
        
        public EffectShapeTorus()
        {
            RadiusMinor = new FloatValue(EffectRules.Shape.TorusRadiusMinor_Default);
            RadiusMajor = new FloatValue(EffectRules.Shape.TorusRadiusMajor_Default);
            Arc = new FloatValue(EffectRules.Shape.Arc_Default);
            Spread = new EffectShapeSpreadRandom();
        }
        public EffectShapeTorus(float radiusMinor, float radiusMajor, float arc, IEffectShapeSpread spread)
        {
            RadiusMinor = new FloatValue(radiusMinor);
            RadiusMajor = new FloatValue(radiusMajor);
            Arc = new FloatValue(arc);
            Spread = spread;
        }
        public EffectShapeTorus(IFloat radiusMinor, IFloat radiusMajor, IFloat arc, IEffectShapeSpread spread)
        {
            RadiusMinor = radiusMinor;
            RadiusMajor = radiusMajor;
            Arc = arc;
            Spread = spread;
        }

        public object Clone() => Copy();
        IEffectShape ICopyable<IEffectShape>.Copy() => new EffectShapeTorus(RadiusMinor.Copy(), RadiusMajor.Copy(), Arc.Copy(), Spread.Copy());
        public EffectShapeTorus Copy() => new(RadiusMinor.Copy(), RadiusMajor.Copy(), Arc.Copy(), Spread.Copy());

        public override bool Equals(object obj) => obj is EffectShapeTorus value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(RadiusMinor, RadiusMajor, Arc, Spread);
        
        public bool Equals(IEffectShape other) => other is EffectShapeTorus value && Equals(value);
        public bool Equals(EffectShapeTorus other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = RadiusMinor.Equals(other.RadiusMinor)
                         && RadiusMajor.Equals(other.RadiusMajor)
                         && Arc.Equals(other.Arc)
                         && Spread.Equals(other.Spread);
            return result;
        }
    }
}