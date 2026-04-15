using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectAngleCurvesBySpeed : IEffectAngle
    {
        [JsonProperty(ModelNames.Curve)]
        public CurveValue Curve { get; set; }
        
        [JsonProperty(ModelNames.Speed + ModelNames.Range)]
        public IVector2 SpeedRange { get; set; }
        
        public EffectAngleType GetModelType() => EffectAngleType.CurvesBySpeed;

        public EffectAngleCurvesBySpeed()
        {
            Curve = new CurveValue(EffectStatic.GetDefaultCurve());
            SpeedRange = new Vector2Value(EffectStatic.Angle_BySpeedRangeDefault);
        }
        public EffectAngleCurvesBySpeed(AnimationCurve curve, Vector2 speedRange)
        {
            Curve = new CurveValue(curve);
            SpeedRange = new Vector2Value(speedRange);
        }
        public EffectAngleCurvesBySpeed(CurveValue curve, IVector2 speedRange)
        {
            Curve = curve;
            SpeedRange = speedRange;
        }
    }
}