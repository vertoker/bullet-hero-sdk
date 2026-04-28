using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectShapeSpreadRandom : IEffectShapeSpread, ICopyable<EffectShapeSpreadRandom>
    {
        [JsonProperty(Names.Spread)]
        public IFloat Spread { get; set; }
        
        public EffectShapeSpreadType GetModelType() => EffectShapeSpreadType.Random;
        public float GetSpread(float time)
        {
            var random = Random.value;
            var spread = Spread.Get();
            if (spread == 0) return random;
            
            random = Mathf.RoundToInt(random / spread) * spread;
            return random;
        }
        
        public EffectShapeSpreadRandom()
        {
            Spread = new FloatValue(EffectStatic.ShapeSpread_SpreadDefault);
        }
        public EffectShapeSpreadRandom(float spread)
        {
            Spread = new FloatValue(spread);
        }
        public EffectShapeSpreadRandom(IFloat spread)
        {
            Spread = spread;
        }
        
        IEffectShapeSpread ICopyable<IEffectShapeSpread>.Copy() => new EffectShapeSpreadRandom(Spread.Copy());
        public EffectShapeSpreadRandom Copy() => new(Spread.Copy());
    }
}