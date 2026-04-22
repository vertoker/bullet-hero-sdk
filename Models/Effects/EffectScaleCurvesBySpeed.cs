using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectScaleCurvesBySpeed : IEffectScale
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
            CurveX = new CurveValue(EffectStatic.GetDefaultCurve());
            CurveY = new CurveValue(EffectStatic.GetDefaultCurve());
            SpeedRange = new Vector2Value(EffectStatic.Scale_BySpeedRangeDefault);
        }
        public EffectScaleCurvesBySpeed(AnimationCurve curveX, AnimationCurve curveY, Vector2 speedRange)
        {
            CurveX = new CurveValue(curveX);
            CurveY = new CurveValue(curveY);
            SpeedRange = new Vector2Value(speedRange);
        }
        public EffectScaleCurvesBySpeed(CurveValue curveX, CurveValue curveY, IVector2 speedRange)
        {
            CurveX = curveX;
            CurveY = curveY;
            SpeedRange = speedRange;
        }
    }
}