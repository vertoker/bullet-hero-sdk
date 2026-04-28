using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectShapeLine : IEffectShape, ICopyable<EffectShapeLine>
    {
        [JsonProperty(Names.Start)]
        public IVector2 Start { get; set; }
        
        [JsonProperty(Names.End)]
        public IVector2 End { get; set; }
        
        [JsonProperty(Names.Spread)]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Line;
        
        public EffectShapeLine()
        {
            Start = new Vector2Value(EffectStatic.Shape_LineStartDefault);
            End = new Vector2Value(EffectStatic.Shape_LineEndDefault);
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

        IEffectShape ICopyable<IEffectShape>.Copy() => new EffectShapeLine(Start.Copy(), End.Copy(), Spread.Copy());
        public EffectShapeLine Copy() => new(Start.Copy(), End.Copy(), Spread.Copy());
    }
}