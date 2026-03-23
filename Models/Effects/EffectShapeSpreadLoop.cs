using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectShapeSpreadLoop : IEffectShapeSpread
    {
        public EffectShapeSpreadType Type => EffectShapeSpreadType.Loop;
    }
}