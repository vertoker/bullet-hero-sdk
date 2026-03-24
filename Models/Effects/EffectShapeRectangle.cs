using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectShapeRectangle : IEffectShape
    {
        // None, inherit TRS from Object
        
        public EffectShapeType GetModelType() => EffectShapeType.Rectangle;
    }
}