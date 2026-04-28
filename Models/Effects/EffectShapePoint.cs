using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectShapePoint : IEffectShape, ICopyable<EffectShapePoint>
    {
        // None, inherit TRS from Object
        
        public EffectShapeType GetModelType() => EffectShapeType.Point;
        
        IEffectShape ICopyable<IEffectShape>.Copy() => new EffectShapePoint();
        public EffectShapePoint Copy() => new();
    }
}