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
    public class EffectAngleCurvesOverLife : IEffectAngle, ICopyable<EffectAngleCurvesOverLife>
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

        IEffectAngle ICopyable<IEffectAngle>.Copy() => new EffectAngleCurvesOverLife(Curve.Copy());
        public EffectAngleCurvesOverLife Copy() => new(Curve.Copy());
    }
}