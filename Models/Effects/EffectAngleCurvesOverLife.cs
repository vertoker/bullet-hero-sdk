using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectAngleCurvesOverLife : IEffectAngle
    {
        public EffectAngleType Type => EffectAngleType.CurvesOverLife;
    }
}