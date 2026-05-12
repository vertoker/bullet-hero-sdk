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
    public class EffectAngleValue : IEffectAngle,
        ICopyable<EffectAngleValue>, IEquatable<EffectAngleValue>
    {
        [RuleNotNull]
        [JsonProperty(Names.Angle)]
        public IFloat Angle { get; set; }
        
        public EffectAngleType GetModelType() => EffectAngleType.Value;

        public EffectAngleValue()
        {
            Angle = new FloatValue(EffectRules.Angle.A_Default);
        }
        public EffectAngleValue(float angle)
        {
            Angle = new FloatValue(angle);
        }
        public EffectAngleValue(IFloat angle)
        {
            Angle = angle;
        }

        public object Clone() => Copy();
        IEffectAngle ICopyable<IEffectAngle>.Copy() => new EffectAngleValue(Angle.Copy());
        public EffectAngleValue Copy() => new(Angle.Copy());

        public override bool Equals(object obj) => obj is EffectAngleValue value && Equals(value);
        public override int GetHashCode() => Angle != null ? Angle.GetHashCode() : 0;
        
        public bool Equals(IEffectAngle other) => other is EffectAngleValue value && Equals(value);
        public bool Equals(EffectAngleValue other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Angle.Equals(other.Angle);
            return result;
        }
    }
}