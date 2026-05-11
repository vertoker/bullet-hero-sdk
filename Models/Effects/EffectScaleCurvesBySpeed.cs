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
    public class EffectScaleCurvesBySpeed : IEffectScale, ICopyable<EffectScaleCurvesBySpeed>
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

        IEffectScale ICopyable<IEffectScale>.Copy() => new EffectScaleCurvesBySpeed(CurveX.Copy(), CurveY.Copy(), SpeedRange.Copy());
        public EffectScaleCurvesBySpeed Copy() => new(CurveX.Copy(), CurveY.Copy(), SpeedRange.Copy());
    }
}