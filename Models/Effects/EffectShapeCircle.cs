using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectShapeCircle : IEffectShape
    {
        [JsonProperty("r")]
        public IFloat Radius { get; set; }
        
        [JsonProperty("t")]
        public IFloat Thickness { get; set; }
        
        [JsonProperty("a")]
        public IFloat Arc { get; set; }
        
        [JsonProperty("spr")]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Circle;

        public EffectShapeCircle()
        {
            Radius = new FloatValue(1f);
            Thickness = new FloatValue(1f);
            Arc = new FloatValue(Mathf.PI * 2f);
            Spread = new EffectShapeSpreadRandom();
        }
        public EffectShapeCircle(float radius, float thickness, float arc, IEffectShapeSpread spread)
        {
            Radius = new FloatValue(radius);
            Thickness = new FloatValue(thickness);
            Arc = new FloatValue(arc);
            Spread = spread;
        }
        public EffectShapeCircle(IFloat radius, IFloat thickness, IFloat arc, IEffectShapeSpread spread)
        {
            Radius = radius;
            Thickness = thickness;
            Arc = arc;
            Spread = spread;
        }
    }
}