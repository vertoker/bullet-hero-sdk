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
    public class EffectScaleValue : IEffectScale,
        ICopyable<EffectScaleValue>, IEquatable<EffectScaleValue>
    {
        [RuleNotNull]
        [JsonProperty(Names.Scale)]
        public IVector2 Scale { get; set; }
        
        public EffectScaleType GetModelType() => EffectScaleType.Value;

        public EffectScaleValue()
        {
            Scale = new Vector2Value(
                EffectRules.Scale.A_X_Default, 
                EffectRules.Scale.A_Y_Default);
        }
        public EffectScaleValue(IVector2 scale)
        {
            Scale = scale;
        }

        public object Clone() => Copy();
        IEffectScale ICopyable<IEffectScale>.Copy() => new EffectScaleValue(Scale.Copy());
        public EffectScaleValue Copy() => new(Scale.Copy());

        public override bool Equals(object obj) => obj is EffectScaleValue value && Equals(value);
        public override int GetHashCode() => Scale != null ? Scale.GetHashCode() : 0;
        
        public bool Equals(IEffectScale other) => other is EffectScaleValue value && Equals(value);
        public bool Equals(EffectScaleValue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Scale.Equals(other.Scale);
            return result;
        }
    }
}