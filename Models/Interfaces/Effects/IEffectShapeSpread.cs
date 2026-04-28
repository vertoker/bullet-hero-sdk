using BHSDK.Models.Enum.Effects;

namespace BHSDK.Models.Interfaces.Effects
{
    public interface IEffectShapeSpread : ICopyable<IEffectShapeSpread>
    {
        public EffectShapeSpreadType GetModelType();
        public float GetSpread(float time);
    }
}