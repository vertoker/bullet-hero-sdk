using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectShapeCone : IEffectShape
    {
        [JsonProperty(ModelNames.TopRadius)]
        public IFloat TopRadius { get; set; }
        
        [JsonProperty(ModelNames.BaseRadius)]
        public IFloat BaseRadius { get; set; }
        
        [JsonProperty(ModelNames.Arc)]
        public IFloat Arc { get; set; }
        
        [JsonProperty(ModelNames.Height)]
        public IFloat Height { get; set; }
        
        [JsonProperty(ModelNames.Spread)]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Cone;
        
        public EffectShapeCone()
        {
            TopRadius = new FloatValue(EffectStatic.Shape_ConeTopRadiusDefault);
            BaseRadius = new FloatValue(EffectStatic.Shape_ConeBaseRadiusDefault);
            Arc = new FloatValue(EffectStatic.Shape_ArcDefault);
            Height = new FloatValue(EffectStatic.Shape_ConeHeightDefault);
            Spread = new EffectShapeSpreadRandom();
        }
        public EffectShapeCone(float topRadius, float baseRadius, float arc, float height, IEffectShapeSpread spread)
        {
            TopRadius = new FloatValue(topRadius);
            BaseRadius = new FloatValue(baseRadius);
            Arc = new FloatValue(arc);
            Height = new FloatValue(height);
            Spread = spread;
        }
        public EffectShapeCone(IFloat topRadius, IFloat baseRadius, IFloat arc, IFloat height, IEffectShapeSpread spread)
        {
            TopRadius = topRadius;
            BaseRadius = baseRadius;
            Arc = arc;
            Height = height;
            Spread = spread;
        }
    }
}