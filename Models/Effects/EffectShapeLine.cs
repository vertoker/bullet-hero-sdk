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
    public class EffectShapeLine : IEffectShape,
        ICopyable<EffectShapeLine>, IEquatable<EffectShapeLine>
    {
        [RuleNotNull]
        [JsonProperty(Names.Start)]
        public IVector2 Start { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.End)]
        public IVector2 End { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Spread)]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Line;
        
        public EffectShapeLine()
        {
            Start = new Vector2Value(
                EffectRules.Shape.LineStart_X_Default,
                EffectRules.Shape.LineStart_Y_Default);
            End = new Vector2Value(
                EffectRules.Shape.LineEnd_X_Default,
                EffectRules.Shape.LineEnd_Y_Default);
            Spread = new EffectShapeSpreadRandom();
        }
        public EffectShapeLine(IVector2 start, IVector2 end, IEffectShapeSpread spread)
        {
            Start = start;
            End = end;
            Spread = spread;
        }

        public object Clone() => Copy();
        IEffectShape ICopyable<IEffectShape>.Copy() => new EffectShapeLine(Start.Copy(), End.Copy(), Spread.Copy());
        public EffectShapeLine Copy() => new(Start.Copy(), End.Copy(), Spread.Copy());

        public override bool Equals(object obj) => obj is EffectShapeLine value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Start, End, Spread);
        
        public bool Equals(IEffectShape other) => other is EffectShapeLine value && Equals(value);
        public bool Equals(EffectShapeLine other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Start.Equals(other.Start)
                         && End.Equals(other.End)
                         && Spread.Equals(other.Spread);
            return result;
        }
    }
}