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
        [JsonProperty("tr")]
        public IFloat TopRadius { get; set; }
        
        [JsonProperty("br")]
        public IFloat BaseRadius { get; set; }
        
        [JsonProperty("h")]
        public IFloat Height { get; set; }
        
        [JsonProperty("a")]
        public IFloat Arc { get; set; }
        
        [JsonProperty("spr")]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Cone;
        
        public EffectShapeCone()
        {
            TopRadius = new FloatValue(0.4f);
            BaseRadius = new FloatValue(1f);
            Height = new FloatValue(1f);
            Arc = new FloatValue(Mathf.PI * 2f);
            Spread = new EffectShapeSpreadRandom();
        }
        public EffectShapeCone(float topRadius, float baseRadius, float height, float arc, IEffectShapeSpread spread)
        {
            TopRadius = new FloatValue(topRadius);
            BaseRadius = new FloatValue(baseRadius);
            Height = new FloatValue(height);
            Arc = new FloatValue(arc);
            Spread = spread;
        }
        public EffectShapeCone(IFloat topRadius, IFloat baseRadius, IFloat height, IFloat arc, IEffectShapeSpread spread)
        {
            TopRadius = topRadius;
            BaseRadius = baseRadius;
            Height = height;
            Arc = arc;
            Spread = spread;
        }
    }
}