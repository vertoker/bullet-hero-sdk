using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectShapeSpreadRandom : IEffectShapeSpread
    {
        [JsonProperty(ModelNames.Spread)]
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
    }
}