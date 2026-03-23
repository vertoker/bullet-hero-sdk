using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectColorRandomUniform : IEffectColor
    {
        public EffectColorType Type => EffectColorType.RandomUniform;
    }
}