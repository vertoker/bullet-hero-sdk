using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectColorGradientBySpeed : IEffectColor
    {
        public EffectColorType Type => EffectColorType.GradientBySpeed;
    }
}