using BHSDK.Models.Enum.Effects;

namespace BHSDK.Models.Interfaces.Effects
{
    public interface IEffectAngle : ICopyable<IEffectAngle>
    {
        public EffectAngleType GetModelType();
    }
}