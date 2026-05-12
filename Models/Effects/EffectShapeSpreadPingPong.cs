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
    public class EffectShapeSpreadPingPong : IEffectShapeSpread, ICopyable<EffectShapeSpreadPingPong>
    {
        [RuleNotNull]
        [JsonProperty(Names.Spread)]
        public IFloat Spread { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.Speed)]
        public IFloat Speed { get; set; }

        public EffectShapeSpreadType GetModelType() => EffectShapeSpreadType.PingPong;
        
        public EffectShapeSpreadPingPong()
        {
            Spread = new FloatValue(EffectRules.ShapeSpread.Spread_Default);
            Speed = new FloatValue(EffectRules.ShapeSpread.Speed_Default);
        }
        public EffectShapeSpreadPingPong(float spread, float speed)
        {
            Spread = new FloatValue(spread);
            Speed = new FloatValue(speed);
        }
        public EffectShapeSpreadPingPong(IFloat spread, IFloat speed)
        {
            Spread = spread;
            Speed = speed;
        }

        public object Clone() => Copy();
        IEffectShapeSpread ICopyable<IEffectShapeSpread>.Copy() => new EffectShapeSpreadPingPong(Spread.Copy(), Speed.Copy());
        public EffectShapeSpreadPingPong Copy() => new(Spread.Copy(), Speed.Copy());
    }
}