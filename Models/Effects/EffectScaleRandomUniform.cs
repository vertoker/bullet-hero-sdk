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
    public class EffectScaleRandomUniform : IEffectScale, IModel<EffectScaleRandomUniform>
    {
        [RuleNotNull]
        [JsonProperty(Names.ScaleX)]
        public IVector2 ScaleA { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.ScaleY)]
        public IVector2 ScaleB { get; set; }

        public EffectScaleType GetModelType() => EffectScaleType.RandomUniform;

        public EffectScaleRandomUniform()
        {
            ScaleA = new Vector2Value(
                EffectRules.Scale.A_X_Default, 
                EffectRules.Scale.A_Y_Default);
            ScaleB = new Vector2Value(
                EffectRules.Scale.B_X_Default, 
                EffectRules.Scale.B_Y_Default);
        }
        public EffectScaleRandomUniform(IVector2 scaleA, IVector2 scaleB)
        {
            ScaleA = scaleA;
            ScaleB = scaleB;
        }
        public void Reset()
        {
            ScaleA = new Vector2Value(
                EffectRules.Scale.A_X_Default, 
                EffectRules.Scale.A_Y_Default);
            ScaleB = new Vector2Value(
                EffectRules.Scale.B_X_Default, 
                EffectRules.Scale.B_Y_Default);
        }

        public object Clone() => Copy();
        IEffectScale ICopyable<IEffectScale>.Copy() => new EffectScaleRandomUniform(ScaleA.Copy(), ScaleB.Copy());
        public EffectScaleRandomUniform Copy() => new(ScaleA.Copy(), ScaleB.Copy());

        public override bool Equals(object obj) => obj is EffectScaleRandomUniform value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(ScaleA, ScaleB);
        
        public bool Equals(IEffectScale other) => other is EffectScaleRandomUniform value && Equals(value);
        public bool Equals(EffectScaleRandomUniform other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = ScaleA.Equals(other.ScaleA)
                         && ScaleB.Equals(other.ScaleB);
            return result;
        }
    }
}