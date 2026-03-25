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
        [JsonProperty(ModelNames.CurveX)]
        public CurveValue CurveX { get; set; }
        
        [JsonProperty(ModelNames.CurveY)]
        public CurveValue CurveY { get; set; }
        
        [JsonProperty(ModelNames.Speed + ModelNames.Range)]
        public IVector2 SpeedRange { get; set; }

        public EffectScaleType GetModelType() => EffectScaleType.CurvesBySpeed;
        
        public EffectScaleCurvesBySpeed()
        {
            CurveX = new CurveValue();
            CurveY = new CurveValue();
            SpeedRange = new Vector2Value(0f, 1f);
        }
        public EffectScaleCurvesBySpeed(CurveValue curveX, CurveValue curveY, Vector2 speedRange)
        {
            CurveX = curveX;
            CurveY = curveY;
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