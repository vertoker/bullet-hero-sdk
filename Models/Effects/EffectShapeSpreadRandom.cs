using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectShapeSpreadRandom : IEffectShapeSpread, ICopyable<EffectShapeSpreadRandom>
    {
        [JsonProperty(Names.Spread)]
        public IFloat Spread { get; set; }
        
        public EffectShapeSpreadType GetModelType() => EffectShapeSpreadType.Random;
        
        public EffectShapeSpreadRandom()
        {
            Spread = new FloatValue(EffectStatic.ShapeSpread_Spread_Default);
        }
        public EffectShapeSpreadRandom(IFloat spread)
        {
            Spread = spread;
        }
        
        IEffectShapeSpread ICopyable<IEffectShapeSpread>.Copy() => new EffectShapeSpreadRandom(Spread.Copy());
        public EffectShapeSpreadRandom Copy() => new(Spread.Copy());
    }
}