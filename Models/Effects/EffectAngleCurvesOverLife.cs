using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectAngleCurvesOverLife : IEffectAngle
    {
        [JsonProperty(ModelNames.Curve)]
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
    }
}