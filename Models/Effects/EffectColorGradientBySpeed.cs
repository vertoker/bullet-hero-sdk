using System;
using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Effects
{
    [RuleContainer]
    public class EffectColorGradientBySpeed : IEffectColor,
        ICopyable<EffectColorGradientBySpeed>, IEquatable<EffectColorGradientBySpeed>
    {
        [JsonProperty(Names.Gradient)]
        public GradientValue Gradient { get; set; }
        
        [JsonProperty(Names.SpeedRange)]
        public IVector2 SpeedRange { get; set; }

        public EffectColorType GetModelType() => EffectColorType.GradientBySpeed;

        public EffectColorGradientBySpeed()
        {
            Gradient = EffectRules.GetGradient_Default();
            SpeedRange = new Vector2Value(
                EffectRules.Color.BySpeedRange_X_Default,
                EffectRules.Color.BySpeedRange_Y_Default);
        }
        public EffectColorGradientBySpeed(GradientValue gradient, IVector2 speedRange)
        {
            Gradient = gradient;
            SpeedRange = speedRange;
        }

        public object Clone() => Copy();
        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorGradientBySpeed(Gradient.Copy(), SpeedRange.Copy());
        public EffectColorGradientBySpeed Copy() => new(Gradient.Copy(), SpeedRange.Copy());

        public override bool Equals(object obj) => obj is EffectColorGradientBySpeed value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Gradient, SpeedRange);
        
        public bool Equals(IEffectColor other) => other is EffectColorGradientBySpeed value && Equals(value);
        public bool Equals(EffectColorGradientBySpeed other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Gradient.Equals(other.Gradient)
                         && SpeedRange.Equals(other.SpeedRange);
            return result;
        }
    }
}