using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectShapeTorus : IEffectShape
    {
        public EffectShapeType Type => EffectShapeType.Torus;
    }
}