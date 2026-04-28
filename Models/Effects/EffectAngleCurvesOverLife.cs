using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectAngleCurvesOverLife : IEffectAngle, ICopyable<EffectAngleCurvesOverLife>
    {
        [JsonProperty(Names.Curve)]
        public CurveValue Curve { get; set; }

        public EffectAngleType GetModelType() => EffectAngleType.CurvesOverLife;
        
        public EffectAngleCurvesOverLife()
        {
            Curve = new CurveValue(EffectStatic.GetDefaultCurve());
        }
        public EffectAngleCurvesOverLife(AnimationCurve curve)
        {
            Curve = new CurveValue(curve);
        }
        public EffectAngleCurvesOverLife(CurveValue curve)
        {
            Curve = curve;
        }

        IEffectAngle ICopyable<IEffectAngle>.Copy() => new EffectAngleCurvesOverLife(Curve.Copy());
        public EffectAngleCurvesOverLife Copy() => new(Curve.Copy());
    }
}