using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectColorRandomPerComponent : IEffectColor
    {
        public EffectColorType Type => EffectColorType.RandomPerComponent;
    }
}