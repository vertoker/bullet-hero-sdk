using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;

namespace BHSDK.Models.Effects
{
    public class EffectShapeSpreadPingPong : IEffectShapeSpread
    {
        public EffectShapeSpreadType Type => EffectShapeSpreadType.PingPong;
    }
}