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
    public class EffectAngleCurvesOverLife : IEffectAngle,
        ICopyable<EffectAngleCurvesOverLife>, IEquatable<EffectAngleCurvesOverLife>
    {
        [RuleNotNull]
        [JsonProperty(Names.Curve)]
        public CurveValue Curve { get; set; }

        public EffectAngleType GetModelType() => EffectAngleType.CurvesOverLife;
        
        public EffectAngleCurvesOverLife()
        {
            Curve = EffectRules.GetCurve_Default();
        }
        public EffectAngleCurvesOverLife(CurveValue curve)
        {
            Curve = curve;
        }

        public object Clone() => Copy();
        IEffectAngle ICopyable<IEffectAngle>.Copy() => new EffectAngleCurvesOverLife(Curve.Copy());
        public EffectAngleCurvesOverLife Copy() => new(Curve.Copy());

        public override bool Equals(object obj) => obj is EffectAngleCurvesOverLife value && Equals(value);
        public override int GetHashCode() => Curve != null ? Curve.GetHashCode() : 0;
        
        public bool Equals(IEffectAngle other) => other is EffectAngleCurvesOverLife value && Equals(value);
        public bool Equals(EffectAngleCurvesOverLife other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Curve.Equals(other.Curve);
            return result;
        }
    }
}