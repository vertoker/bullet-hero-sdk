using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectAngleRandomPerComponent : IEffectAngle
    {
        public EffectAngleType Type => EffectAngleType.RandomPerComponent;
    }
}