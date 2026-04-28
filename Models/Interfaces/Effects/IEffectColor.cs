using BHSDK.Models.Enum.Effects;

namespace BHSDK.Models.Interfaces.Effects
{
    public interface IEffectColor : ICopyable<IEffectColor>
    {
        public EffectColorType GetModelType();
    }
}