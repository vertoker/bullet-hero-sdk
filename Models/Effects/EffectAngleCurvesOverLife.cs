using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectAngleCurvesOverLife : IEffectAngle
    {
        [JsonProperty("c")]
        public CurveValue Curve { get; set; }

        public EffectAngleType GetModelType() => EffectAngleType.CurvesOverLife;
        
        public EffectAngleCurvesOverLife()
        {
            Curve = new CurveValue();
        }
        public EffectAngleCurvesOverLife(CurveValue curve)
        {
            Curve = curve;
        }
    }
}