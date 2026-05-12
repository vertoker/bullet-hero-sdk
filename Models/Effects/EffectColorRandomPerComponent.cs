using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    [RuleContainer]
    public class EffectColorRandomPerComponent : IEffectColor, ICopyable<EffectColorRandomPerComponent>
    {
        [RuleNotNull]
        [JsonProperty(Names.ColorA)]
        public IColor ColorA { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.ColorB)]
        public IColor ColorB { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.RandomPerComponent;

        public EffectColorRandomPerComponent()
        {
            ColorA = new ColorValue(
                EffectRules.Color.A_R_Default,
                EffectRules.Color.A_G_Default,
                EffectRules.Color.A_B_Default,
                EffectRules.Color.A_A_Default);
            ColorB = new ColorValue(
                EffectRules.Color.B_R_Default,
                EffectRules.Color.B_G_Default,
                EffectRules.Color.B_B_Default,
                EffectRules.Color.B_A_Default);
        }
        public EffectColorRandomPerComponent(IColor colorA, IColor colorB)
        {
            ColorA = colorA;
            ColorB = colorB;
        }

        public object Clone() => Copy();
        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorRandomPerComponent(ColorA.Copy(), ColorB.Copy());
        public EffectColorRandomPerComponent Copy() => new(ColorA.Copy(), ColorB.Copy());
    }
}