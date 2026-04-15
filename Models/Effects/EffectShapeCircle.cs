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
        [JsonProperty(ModelNames.Radius)]
        public IFloat Radius { get; set; }
        
        [JsonProperty(ModelNames.Thickness)]
        public IFloat Thickness { get; set; }
        
        [JsonProperty(ModelNames.Arc)]
        public IFloat Arc { get; set; }
        
        [JsonProperty(ModelNames.Spread)]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Circle;

        public EffectShapeCircle()
        {
            Radius = new FloatValue(EffectStatic.Shape_CircleRadiusDefault);
            Thickness = new FloatValue(EffectStatic.Shape_CircleThicknessDefault);
            Arc = new FloatValue(EffectStatic.Shape_ArcDefault);
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