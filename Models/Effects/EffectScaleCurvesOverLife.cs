using System;
using BH.SDK.Models.Enum.Effects;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Effects;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Effects
{
    [RuleContainer]
    public class EffectScaleCurvesOverLife : IEffectScale, IModel<EffectScaleCurvesOverLife>
    {
        [RuleNotNull]
        [JsonProperty(Names.CurveX)]
        public CurveValue CurveX { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.CurveY)]
        public CurveValue CurveY { get; set; }
        
        public EffectScaleType GetModelType() => EffectScaleType.CurvesOverLife;
        
        public EffectScaleCurvesOverLife()
        {
            CurveX = EffectRules.GetCurve_Default();
            CurveY = EffectRules.GetCurve_Default();
        }
        public EffectScaleCurvesOverLife(CurveValue curveX, CurveValue curveY)
        {
            CurveX = curveX;
            CurveY = curveY;
        }
        public void Reset()
        {
            CurveX = EffectRules.GetCurve_Default();
            CurveY = EffectRules.GetCurve_Default();
        }

        public object Clone() => Copy();
        IEffectScale ICopyable<IEffectScale>.Copy() => new EffectScaleCurvesOverLife(CurveX.Copy(), CurveY.Copy());
        public EffectScaleCurvesOverLife Copy() => new(CurveX.Copy(), CurveY.Copy());

        public override bool Equals(object obj) => obj is EffectScaleCurvesOverLife value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(CurveX, CurveY);
        
        public bool Equals(IEffectScale other) => other is EffectScaleCurvesOverLife value && Equals(value);
        public bool Equals(EffectScaleCurvesOverLife other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = CurveX.Equals(other.CurveX)
                         && CurveY.Equals(other.CurveY);
            return result;
        }
    }
}