using System;
using BH.SDK.Models.Enum.Effects;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Effects;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Effects
{
    [RuleContainer]
    public class EffectColorGradientRandom : IEffectColor, IModel<EffectColorGradientRandom>
    {
        [RuleNotNull]
        [JsonProperty(Names.Gradient)]
        public GradientValue Gradient { get; set; }

        public EffectColorType GetModelType() => EffectColorType.GradientRandom;

        public EffectColorGradientRandom()
        {
            Gradient = EffectRules.GetGradient_Default();
        }
        public EffectColorGradientRandom(GradientValue gradient)
        {
            Gradient = gradient;
        }
        public void Reset()
        {
            Gradient = EffectRules.GetGradient_Default();
        }

        public object Clone() => Copy();
        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorGradientRandom(Gradient.Copy());
        public EffectColorGradientRandom Copy() => new(Gradient.Copy());

        public override bool Equals(object obj) => obj is EffectColorGradientRandom value && Equals(value);
        public override int GetHashCode() => Gradient != null ? Gradient.GetHashCode() : 0;
        
        public bool Equals(IEffectColor other) => other is EffectColorGradientRandom value && Equals(value);
        public bool Equals(EffectColorGradientRandom other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Gradient.Equals(other.Gradient);
            return result;
        }
    }
}