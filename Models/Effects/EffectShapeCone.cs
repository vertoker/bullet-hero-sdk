using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectShapeCone : IEffectShape
    {
        public EffectShapeType Type => EffectShapeType.Cone;
    }
}