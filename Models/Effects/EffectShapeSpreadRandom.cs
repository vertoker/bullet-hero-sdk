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
    public class EffectShapeSpreadRandom : IEffectShapeSpread, ICopyable<EffectShapeSpreadRandom>
    {
        [RuleNotNull]
        [JsonProperty(Names.Spread)]
        public IFloat Spread { get; set; }
        
        public EffectShapeSpreadType GetModelType() => EffectShapeSpreadType.Random;
        
        public EffectShapeSpreadRandom()
        {
            Spread = new FloatValue(EffectRules.ShapeSpread.Spread_Default);
        }
        public EffectShapeSpreadRandom(IFloat spread)
        {
            Spread = spread;
        }
        
        public object Clone() => Copy();
        IEffectShapeSpread ICopyable<IEffectShapeSpread>.Copy() => new EffectShapeSpreadRandom(Spread.Copy());
        public EffectShapeSpreadRandom Copy() => new(Spread.Copy());
    }
}