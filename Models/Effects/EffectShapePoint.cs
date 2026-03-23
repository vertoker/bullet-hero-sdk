using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectShapePoint : IEffectShape
    {
        // None, inherit TRS from Instance
        
        public EffectShapeType GetModelType() => EffectShapeType.Point;
    }
}