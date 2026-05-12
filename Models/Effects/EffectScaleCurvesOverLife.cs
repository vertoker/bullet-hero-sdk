using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    [RuleContainer]
    public class EffectScaleCurvesOverLife : IEffectScale, ICopyable<EffectScaleCurvesOverLife>
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

        public object Clone() => Copy();
        IEffectScale ICopyable<IEffectScale>.Copy() => new EffectScaleCurvesOverLife(CurveX.Copy(), CurveY.Copy());
        public EffectScaleCurvesOverLife Copy() => new(CurveX.Copy(), CurveY.Copy());
    }
}