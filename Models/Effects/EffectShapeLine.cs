using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectShapeLine : IEffectShape
    {
        [JsonProperty("s")]
        public IVector2 Start { get; set; }
        
        [JsonProperty("e")]
        public IVector2 End { get; set; }
        
        [JsonProperty("spr")]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Line;
        
        public EffectShapeLine()
        {
            Start = new Vector2Value(0f, 0f);
            End = new Vector2Value(1f, 0f);
            Spread = new EffectShapeSpreadRandom();
        }
        public EffectShapeLine(Vector2 start, Vector2 end, IEffectShapeSpread spread)
        {
            Start = new Vector2Value(start);
            End = new Vector2Value(end);
            Spread = spread;
        }
        public EffectShapeLine(IVector2 start, IVector2 end, IEffectShapeSpread spread)
        {
            Start = start;
            End = end;
            Spread = spread;
        }
    }
}