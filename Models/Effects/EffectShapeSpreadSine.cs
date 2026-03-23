using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectShapeSpreadSine : IEffectShapeSpread
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
    }
}