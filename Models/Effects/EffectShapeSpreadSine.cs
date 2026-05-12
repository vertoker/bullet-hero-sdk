using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Rules.Attributes;

namespace BHSDK.Models.Effects
{
    [RuleContainer]
    public class EffectShapeSpreadSine : IEffectShapeSpread, ICopyable<EffectShapeSpreadSine>
    {
        public EffectShapeSpreadType GetModelType() => EffectShapeSpreadType.Sine;
        
        public object Clone() => Copy();
        IEffectShapeSpread ICopyable<IEffectShapeSpread>.Copy() => new EffectShapeSpreadSine();
        public EffectShapeSpreadSine Copy() => new();
    }
}