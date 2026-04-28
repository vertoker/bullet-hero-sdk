using BHSDK.Models.Enum.Effects;

namespace BHSDK.Models.Interfaces.Effects
{
    public interface IEffectScale : ICopyable<IEffectScale>
    {
        public EffectScaleType GetModelType();
    }
}