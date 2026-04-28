using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectShapeSpreadSine : IEffectShapeSpread, ICopyable<EffectShapeSpreadSine>
    {
        public EffectShapeSpreadType GetModelType() => EffectShapeSpreadType.Sine;
        public float GetSpread(float time)
        {
            var random = time * 2f;
            random = Mathf.Sin(random);
            random += 1f;
            random *= 2f;
            return random;
        }
        
        IEffectShapeSpread ICopyable<IEffectShapeSpread>.Copy() => new EffectShapeSpreadSine();
        public EffectShapeSpreadSine Copy() => new();
    }
}