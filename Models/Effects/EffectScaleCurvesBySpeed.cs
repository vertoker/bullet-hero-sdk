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
    public class EffectScaleCurvesBySpeed : IEffectScale,
        ICopyable<EffectScaleCurvesBySpeed>, IEquatable<EffectScaleCurvesBySpeed>
    {
        [RuleNotNull]
        [JsonProperty(Names.CurveX)]
        public CurveValue CurveX { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.CurveY)]
        public CurveValue CurveY { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.SpeedRange)]
        public IVector2 SpeedRange { get; set; }

        public EffectScaleType GetModelType() => EffectScaleType.CurvesBySpeed;
        
        public EffectScaleCurvesBySpeed()
        {
            CurveX = EffectRules.GetCurve_Default();
            CurveY = EffectRules.GetCurve_Default();
            SpeedRange = new Vector2Value(
                EffectRules.Scale.BySpeedRange_X_Default,
                EffectRules.Scale.BySpeedRange_Y_Default);
        }
        public EffectScaleCurvesBySpeed(CurveValue curveX, CurveValue curveY, IVector2 speedRange)
        {
            CurveX = curveX;
            CurveY = curveY;
            SpeedRange = speedRange;
        }

        public object Clone() => Copy();
        IEffectScale ICopyable<IEffectScale>.Copy() => new EffectScaleCurvesBySpeed(CurveX.Copy(), CurveY.Copy(), SpeedRange.Copy());
        public EffectScaleCurvesBySpeed Copy() => new(CurveX.Copy(), CurveY.Copy(), SpeedRange.Copy());

        public override bool Equals(object obj) => obj is EffectScaleCurvesBySpeed value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(CurveX, CurveY, SpeedRange);
        
        public bool Equals(IEffectScale other) => other is EffectScaleCurvesBySpeed value && Equals(value);
        public bool Equals(EffectScaleCurvesBySpeed other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = CurveX.Equals(other.CurveX)
                         && CurveY.Equals(other.CurveY)
                         && SpeedRange.Equals(other.SpeedRange);
            return result;
        }
    }
}