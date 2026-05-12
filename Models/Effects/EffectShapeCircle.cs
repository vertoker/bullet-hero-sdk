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
    public class EffectShapeCircle : IEffectShape, ICopyable<EffectShapeCircle>
    {
        [RuleNotNull, RuleIFloatMin(EffectRules.Shape.CircleRadius_Min)]
        [JsonProperty(Names.Radius)]
        public IFloat Radius { get; set; }
        
        [RuleNotNull, RuleIFloatInRange(EffectRules.Shape.CircleThickness_Min, EffectRules.Shape.CircleThickness_Max)]
        [JsonProperty(Names.Thickness)]
        public IFloat Thickness { get; set; }
        
        [RuleNotNull, RuleIFloatInRange(EffectRules.Shape.Arc_Min, EffectRules.Shape.Arc_Max)]
        [JsonProperty(Names.Arc)]
        public IFloat Arc { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Spread)]
        public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType GetModelType() => EffectShapeType.Circle;

        public EffectShapeCircle()
        {
            Radius = new FloatValue(EffectRules.Shape.CircleRadius_Default);
            Thickness = new FloatValue(EffectRules.Shape.CircleThickness_Default);
            Arc = new FloatValue(EffectRules.Shape.Arc_Default);
            Spread = new EffectShapeSpreadRandom();
        }
        public EffectShapeCircle(IFloat radius, IFloat thickness, IFloat arc, IEffectShapeSpread spread)
        {
            Radius = radius;
            Thickness = thickness;
            Arc = arc;
            Spread = spread;
        }

        public object Clone() => Copy();
        IEffectShape ICopyable<IEffectShape>.Copy() => new EffectShapeCircle(Radius.Copy(), Thickness.Copy(), Arc.Copy(), Spread.Copy());
        public EffectShapeCircle Copy() => new(Radius.Copy(), Thickness.Copy(), Arc.Copy(), Spread.Copy());
    }
}