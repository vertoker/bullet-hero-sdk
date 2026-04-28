using BHSDK.Models.Enum.Effects;

namespace BHSDK.Models.Interfaces.Effects
{
    public interface IEffectShape : ICopyable<IEffectShape>
    {
        public EffectShapeType GetModelType();
    }
}