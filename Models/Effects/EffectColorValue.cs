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
    public class EffectColorValue : IEffectColor,
        ICopyable<EffectColorValue>, IEquatable<EffectColorValue>
    {
        [RuleNotNull]
        [JsonProperty(Names.Color)]
        public IColor Color { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.Value;

        public EffectColorValue()
        {
            Color = new ColorValue(
                EffectRules.Color.A_R_Default,
                EffectRules.Color.A_G_Default,
                EffectRules.Color.A_B_Default,
                EffectRules.Color.A_A_Default);
        }
        public EffectColorValue(IColor color)
        {
            Color = color;
        }

        public object Clone() => Copy();
        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorValue(Color.Copy());
        public EffectColorValue Copy() => new(Color.Copy());
        
        public bool Equals(IEffectColor other) => other is EffectColorValue value && Equals(value);

        public override bool Equals(object obj) => obj is EffectColorValue value && Equals(value);
        public override int GetHashCode() => Color != null ? Color.GetHashCode() : 0;

        public bool Equals(EffectColorValue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Color.Equals(other.Color);
            return result;
        }
    }
}