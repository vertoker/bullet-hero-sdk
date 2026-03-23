using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectShapeSpreadSine : IEffectShapeSpread
    {
        public EffectShapeSpreadType Type => EffectShapeSpreadType.Sine;
    }
}