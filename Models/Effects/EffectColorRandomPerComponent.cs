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
    public class EffectColorRandomPerComponent : IEffectColor,
        ICopyable<EffectColorRandomPerComponent>, IEquatable<EffectColorRandomPerComponent>
    {
        [RuleNotNull]
        [JsonProperty(Names.ColorA)]
        public IColor ColorA { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.ColorB)]
        public IColor ColorB { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.RandomPerComponent;

        public EffectColorRandomPerComponent()
        {
            ColorA = new ColorValue(
                EffectRules.Color.A_R_Default,
                EffectRules.Color.A_G_Default,
                EffectRules.Color.A_B_Default,
                EffectRules.Color.A_A_Default);
            ColorB = new ColorValue(
                EffectRules.Color.B_R_Default,
                EffectRules.Color.B_G_Default,
                EffectRules.Color.B_B_Default,
                EffectRules.Color.B_A_Default);
        }
        public EffectColorRandomPerComponent(IColor colorA, IColor colorB)
        {
            ColorA = colorA;
            ColorB = colorB;
        }

        public object Clone() => Copy();
        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorRandomPerComponent(ColorA.Copy(), ColorB.Copy());
        public EffectColorRandomPerComponent Copy() => new(ColorA.Copy(), ColorB.Copy());

        public override bool Equals(object obj) => obj is EffectColorRandomPerComponent value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(ColorA, ColorB);
        
        public bool Equals(IEffectColor other) => other is EffectColorRandomPerComponent value && Equals(value);
        public bool Equals(EffectColorRandomPerComponent other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = ColorA.Equals(other.ColorA)
                         && ColorB.Equals(other.ColorB);
            return result;
        }
    }
}