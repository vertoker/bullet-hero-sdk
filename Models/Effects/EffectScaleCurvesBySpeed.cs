using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectScaleCurvesBySpeed : IEffectScale, ICopyable<EffectScaleCurvesBySpeed>
    {
        [JsonProperty(Names.CurveX)]
        public CurveValue CurveX { get; set; }
        
        [JsonProperty(Names.CurveY)]
        public CurveValue CurveY { get; set; }
        
        [JsonProperty(Names.SpeedRange)]
        public IVector2 SpeedRange { get; set; }

        public EffectScaleType GetModelType() => EffectScaleType.CurvesBySpeed;
        
        public EffectScaleCurvesBySpeed()
        {
            CurveX = EffectStatic.GetCurve_Default();
            CurveY = EffectStatic.GetCurve_Default();
            SpeedRange = new Vector2Value(
                EffectStatic.Scale_BySpeedRange_X_Default,
                EffectStatic.Scale_BySpeedRange_Y_Default);
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