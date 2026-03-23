using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectShapeSpreadRandom : IEffectShapeSpread
    {
        public EffectShapeSpreadType Type => EffectShapeSpreadType.Random;
    }
}