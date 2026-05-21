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
    public class EffectAngleRandomUniform : IEffectAngle,
        ICopyable<EffectAngleRandomUniform>, IEquatable<EffectAngleRandomUniform>
    {
        [RuleNotNull]
        [JsonProperty(Names.AngleA)]
        public IFloat AngleA { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.AngleB)]
        public IFloat AngleB { get; set; }

        public EffectAngleType GetModelType() => EffectAngleType.RandomUniform;

        public EffectAngleRandomUniform()
        {
            AngleA = new FloatValue(EffectRules.Angle.A_Default);
            AngleB = new FloatValue(EffectRules.Angle.B_Default);
        }
        public EffectAngleRandomUniform(float angleA, float angleB)
        {
            AngleA = new FloatValue(angleA);
            AngleB = new FloatValue(angleB);
        }
        public EffectAngleRandomUniform(IFloat angleA, IFloat angleB)
        {
            AngleA = angleA;
            AngleB = angleB;
        }

        public object Clone() => Copy();
        IEffectAngle ICopyable<IEffectAngle>.Copy() => new EffectAngleRandomUniform(AngleA.Copy(), AngleB.Copy());
        public EffectAngleRandomUniform Copy() => new(AngleA.Copy(), AngleB.Copy());

        public override bool Equals(object obj) => obj is EffectAngleRandomUniform value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(AngleA, AngleB);
        
        public bool Equals(IEffectAngle other) => other is EffectAngleRandomUniform value && Equals(value);
        public bool Equals(EffectAngleRandomUniform other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = AngleA.Equals(other.AngleA)
                         && AngleB.Equals(other.AngleB);
            return result;
        }
    }
}