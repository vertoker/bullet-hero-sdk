using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectScaleRandomUniform : IEffectScale
    {
        public EffectScaleType Type => EffectScaleType.RandomUniform;
    }
}