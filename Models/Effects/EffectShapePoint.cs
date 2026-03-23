using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectShapePoint : IEffectShape
    {
        // None, spawn where Instance located
        
        public EffectShapeType Type => EffectShapeType.Point;
    }
}