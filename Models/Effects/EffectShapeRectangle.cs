using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectShapeRectangle : IEffectShape
    {
        // None, inherit TRS from Instance
        
        public EffectShapeType GetModelType() => EffectShapeType.Rectangle;
    }
}