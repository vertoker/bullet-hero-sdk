using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectScaleValue : IEffectScale
    {
        public EffectScaleType Type => EffectScaleType.Value;
    }
}