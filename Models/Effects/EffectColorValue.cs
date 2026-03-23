using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectColorValue : IEffectColor
    {
        public EffectColorType Type => EffectColorType.Value;
    }
}