using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectShapeLine : IEffectShape
    {
        public EffectShapeType Type => EffectShapeType.Line;
    }
}