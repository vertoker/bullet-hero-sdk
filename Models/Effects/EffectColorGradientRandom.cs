using System;
using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Effects
{
    [RuleContainer]
    public class EffectColorGradientRandom : IEffectColor,
        ICopyable<EffectColorGradientRandom>, IEquatable<EffectColorGradientRandom>
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