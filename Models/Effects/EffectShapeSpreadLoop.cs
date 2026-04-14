using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectShapeSpreadLoop : IEffectShapeSpread
    {
        [JsonProperty(ModelNames.Spread)]
        public IFloat Spread { get; set; }
        
        [JsonProperty(ModelNames.Speed)]
        public IFloat Speed { get; set; }
        
        public EffectShapeSpreadType GetModelType() => EffectShapeSpreadType.Loop;
        public float GetSpread(float time)
        {
            var random = time * Speed.Get();
            
            var spread = Spread.Get();
            if (spread != 0)
                random = Mathf.RoundToInt(random / spread) * spread;

            random = Mathf.Repeat(random, 1);
            return random;
        }

        public EffectShapeSpreadLoop()
        {
            Spread = new FloatValue(EffectStatic.ShapeSpreadDefault);
            Speed = new FloatValue(EffectStatic.ShapeSpreadSpeedDefault);
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
    }
}