using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectShapeSpreadLoop : IEffectShapeSpread, ICopyable<EffectShapeSpreadLoop>
    {
        [JsonProperty(Names.Spread)]
        public IFloat Spread { get; set; }
        
        [JsonProperty(Names.Speed)]
        public IFloat Speed { get; set; }
        
        public EffectShapeSpreadType GetModelType() => EffectShapeSpreadType.Loop;

        public EffectShapeSpreadLoop()
        {
            Spread = new FloatValue(EffectStatic.ShapeSpread_Spread_Default);
            Speed = new FloatValue(EffectStatic.ShapeSpread_Speed_Default);
        }
        public EffectShapeSpreadLoop(float spread, float speed)
        {
            Spread = new FloatValue(spread);
            Speed = new FloatValue(speed);
        }
        public EffectShapeSpreadLoop(IFloat spread, IFloat speed)
        {
            Spread = spread;
            Speed = speed;
        }

        IEffectShapeSpread ICopyable<IEffectShapeSpread>.Copy() => new EffectShapeSpreadLoop(Spread.Copy(), Speed.Copy());
        public EffectShapeSpreadLoop Copy() => new(Spread.Copy(), Speed.Copy());
    }
}