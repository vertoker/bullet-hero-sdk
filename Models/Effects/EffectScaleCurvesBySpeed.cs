using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectScaleCurvesBySpeed : IEffectScale
    {
        public EffectScaleType Type => EffectScaleType.CurvesBySpeed;
    }
}