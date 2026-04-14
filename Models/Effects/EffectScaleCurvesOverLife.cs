using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectScaleCurvesOverLife : IEffectScale
    {
        [JsonProperty(ModelNames.CurveX)]
        public CurveValue CurveX { get; set; }
        
        [JsonProperty(ModelNames.CurveY)]
        public CurveValue CurveY { get; set; }
        
        public EffectScaleType GetModelType() => EffectScaleType.CurvesOverLife;
        
        public EffectScaleCurvesOverLife()
        {
            CurveX = new CurveValue(EffectStatic.GetDefaultCurve());
            CurveY = new CurveValue(EffectStatic.GetDefaultCurve());
        }
        public EffectScaleCurvesOverLife(AnimationCurve curveX, AnimationCurve curveY)
        {
            CurveX = new CurveValue(curveX);
            CurveY = new CurveValue(curveY);
        }
        public EffectScaleCurvesOverLife(CurveValue curveX, CurveValue curveY)
        {
            CurveX = curveX;
            CurveY = curveY;
        }
    }
}