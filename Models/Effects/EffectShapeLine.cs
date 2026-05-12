using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    [RuleContainer]
    public class EffectShapeLine : IEffectShape, ICopyable<EffectShapeLine>
    {
        [RuleNotNull]
        [JsonProperty(Names.Start)]
        public IVector2 Start { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.End)]
        public IVector2 End { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Spread)]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Line;
        
        public EffectShapeLine()
        {
            Start = new Vector2Value(
                EffectRules.Shape.LineStart_X_Default,
                EffectRules.Shape.LineStart_Y_Default);
            End = new Vector2Value(
                EffectRules.Shape.LineEnd_X_Default,
                EffectRules.Shape.LineEnd_Y_Default);
            Spread = new EffectShapeSpreadRandom();
        }
        public EffectShapeLine(IVector2 start, IVector2 end, IEffectShapeSpread spread)
        {
            Start = start;
            End = end;
            Spread = spread;
        }

        public object Clone() => Copy();
        IEffectShape ICopyable<IEffectShape>.Copy() => new EffectShapeLine(Start.Copy(), End.Copy(), Spread.Copy());
        public EffectShapeLine Copy() => new(Start.Copy(), End.Copy(), Spread.Copy());
    }
}