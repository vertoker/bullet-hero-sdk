using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectScaleCurvesOverLife : IEffectScale, ICopyable<EffectScaleCurvesOverLife>
    {
        [JsonProperty(Names.CurveX)]
        public CurveValue CurveX { get; set; }
        
        [JsonProperty(Names.CurveY)]
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

        IEffectScale ICopyable<IEffectScale>.Copy() => new EffectScaleCurvesOverLife(CurveX.Copy(), CurveY.Copy());
        public EffectScaleCurvesOverLife Copy() => new(CurveX.Copy(), CurveY.Copy());
    }
}