using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectAngleRandomUniform : IEffectAngle
    {
        public EffectAngleType Type => EffectAngleType.RandomUniform;
    }
}