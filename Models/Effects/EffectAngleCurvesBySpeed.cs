using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectAngleCurvesBySpeed : IEffectAngle
    {
        public EffectAngleType Type => EffectAngleType.CurvesBySpeed;
    }
}