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
    public class EffectShapeSpreadRandom : IEffectShapeSpread,
        ICopyable<EffectShapeSpreadRandom>, IEquatable<EffectShapeSpreadRandom>
    {
        [RuleNotNull]
        [JsonProperty(Names.Spread)]
        public IFloat Spread { get; set; }
        
        public EffectShapeSpreadType GetModelType() => EffectShapeSpreadType.Random;
        
        public EffectShapeSpreadRandom()
        {
            Spread = new FloatValue(EffectRules.ShapeSpread.Spread_Default);
        }
        public EffectShapeSpreadRandom(IFloat spread)
        {
            Spread = spread;
        }
        
        public object Clone() => Copy();
        IEffectShapeSpread ICopyable<IEffectShapeSpread>.Copy() => new EffectShapeSpreadRandom(Spread.Copy());
        public EffectShapeSpreadRandom Copy() => new(Spread.Copy());
        
        public bool Equals(IEffectShapeSpread other) => other is EffectShapeSpreadRandom value && Equals(value);
        public bool Equals(EffectShapeSpreadRandom other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Spread.Equals(other.Spread);
            return result;
        }

        public override bool Equals(object obj) => obj is EffectShapeSpreadRandom value && Equals(value);
        public override int GetHashCode() => Spread != null ? Spread.GetHashCode() : 0;
    }
}