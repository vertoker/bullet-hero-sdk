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
    public class EffectColorRandomPerComponent : IEffectColor, IModel<EffectColorRandomPerComponent>
    {
        [RuleNotNull]
        [JsonProperty(Names.ColorA)]
        public IColor4 Color4A { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.ColorB)]
        public IColor4 Color4B { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.RandomPerComponent;

        public EffectColorRandomPerComponent()
        {
            Color4A = new Color4Value(
                EffectRules.Color.A_R_Default,
                EffectRules.Color.A_G_Default,
                EffectRules.Color.A_B_Default,
                EffectRules.Color.A_A_Default);
            Color4B = new Color4Value(
                EffectRules.Color.B_R_Default,
                EffectRules.Color.B_G_Default,
                EffectRules.Color.B_B_Default,
                EffectRules.Color.B_A_Default);
        }
        public EffectColorRandomPerComponent(IColor4 color4A, IColor4 color4B)
        {
            Color4A = color4A;
            Color4B = color4B;
        }
        public void Reset()
        {
            Color4A = new Color4Value(
                EffectRules.Color.A_R_Default,
                EffectRules.Color.A_G_Default,
                EffectRules.Color.A_B_Default,
                EffectRules.Color.A_A_Default);
            Color4B = new Color4Value(
                EffectRules.Color.B_R_Default,
                EffectRules.Color.B_G_Default,
                EffectRules.Color.B_B_Default,
                EffectRules.Color.B_A_Default);
        }

        public object Clone() => Copy();
        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorRandomPerComponent(Color4A.Copy(), Color4B.Copy());
        public EffectColorRandomPerComponent Copy() => new(Color4A.Copy(), Color4B.Copy());

        public override bool Equals(object obj) => obj is EffectColorRandomPerComponent value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Color4A, Color4B);
        
        public bool Equals(IEffectColor other) => other is EffectColorRandomPerComponent value && Equals(value);
        public bool Equals(EffectColorRandomPerComponent other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Color4A.Equals(other.Color4A)
                         && Color4B.Equals(other.Color4B);
            return result;
        }
    }
}