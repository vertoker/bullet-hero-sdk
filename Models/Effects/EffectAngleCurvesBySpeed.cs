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
    public class EffectAngleCurvesBySpeed : IEffectAngle,
        ICopyable<EffectAngleCurvesBySpeed>, IEquatable<EffectAngleCurvesBySpeed>
    {
        [RuleNotNull]
        [JsonProperty(Names.Curve)]
        public CurveValue Curve { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.SpeedRange)]
        public IVector2 SpeedRange { get; set; }
        
        public EffectAngleType GetModelType() => EffectAngleType.CurvesBySpeed;

        public EffectAngleCurvesBySpeed()
        {
            Curve = EffectRules.GetCurve_Default();
            SpeedRange = new Vector2Value(
                EffectRules.Angle.BySpeedRange_X_Default,
                EffectRules.Angle.BySpeedRange_Y_Default);
        }
        public EffectAngleCurvesBySpeed(CurveValue curve, IVector2 speedRange)
        {
            Curve = curve;
            SpeedRange = speedRange;
        }

        public object Clone() => Copy();
        IEffectAngle ICopyable<IEffectAngle>.Copy() => new EffectAngleCurvesBySpeed(Curve.Copy(), SpeedRange.Copy());
        public EffectAngleCurvesBySpeed Copy() => new(Curve.Copy(), SpeedRange.Copy());

        public override bool Equals(object obj) => obj is EffectAngleCurvesBySpeed value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Curve, SpeedRange);
        
        public bool Equals(IEffectAngle other) => other is EffectAngleCurvesBySpeed value && Equals(value);
        public bool Equals(EffectAngleCurvesBySpeed other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Curve.Equals(other.Curve)
                         && SpeedRange.Equals(other.SpeedRange);
            return result;
        }
    }
}