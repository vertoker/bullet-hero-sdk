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
    public class EffectShapeSpreadPingPong : IEffectShapeSpread, IModel<EffectShapeSpreadPingPong>
    {
        [RuleNotNull]
        [JsonProperty(Names.Spread)]
        public IFloat Spread { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Speed)]
        public IFloat Speed { get; set; }

        public EffectShapeSpreadType GetModelType() => EffectShapeSpreadType.PingPong;
        
        public EffectShapeSpreadPingPong()
        {
            Spread = new FloatValue(EffectRules.ShapeSpread.Spread_Default);
            Speed = new FloatValue(EffectRules.ShapeSpread.Speed_Default);
        }
        public EffectShapeSpreadPingPong(float spread, float speed)
        {
            Spread = new FloatValue(spread);
            Speed = new FloatValue(speed);
        }
        public EffectShapeSpreadPingPong(IFloat spread, IFloat speed)
        {
            Spread = spread;
            Speed = speed;
        }
        public void Reset()
        {
            Spread = new FloatValue(EffectRules.ShapeSpread.Spread_Default);
            Speed = new FloatValue(EffectRules.ShapeSpread.Speed_Default);
        }

        public object Clone() => Copy();
        IEffectShapeSpread ICopyable<IEffectShapeSpread>.Copy() => new EffectShapeSpreadPingPong(Spread.Copy(), Speed.Copy());
        public EffectShapeSpreadPingPong Copy() => new(Spread.Copy(), Speed.Copy());

        public override bool Equals(object obj) => obj is EffectShapeSpreadPingPong value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Spread, Speed);
        
        public bool Equals(IEffectShapeSpread other) => other is EffectShapeSpreadPingPong value && Equals(value);
        public bool Equals(EffectShapeSpreadPingPong other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Spread.Equals(other.Spread)
                         && Speed.Equals(other.Speed);
            return result;
        }
    }
}