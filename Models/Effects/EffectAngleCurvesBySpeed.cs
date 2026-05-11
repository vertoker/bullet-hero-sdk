using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    [RuleContainer]
    public class EffectAngleCurvesBySpeed : IEffectAngle, ICopyable<EffectAngleCurvesBySpeed>
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

        IEffectAngle ICopyable<IEffectAngle>.Copy() => new EffectAngleCurvesBySpeed(Curve.Copy(), SpeedRange.Copy());
        public EffectAngleCurvesBySpeed Copy() => new(Curve.Copy(), SpeedRange.Copy());
    }
}