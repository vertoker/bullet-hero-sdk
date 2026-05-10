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
    public class EffectColorRandomUniform : IEffectColor, ICopyable<EffectColorRandomUniform>
    {
        [RuleNotNull]
        [JsonProperty(Names.ColorA)]
        public IColor ColorA { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.ColorB)]
        public IColor ColorB { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.RandomUniform;

        public EffectColorRandomUniform()
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
        public EffectColorRandomUniform(IColor colorA, IColor colorB)
        {
            ColorA = colorA;
            ColorB = colorB;
        }

        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorRandomUniform(ColorA.Copy(), ColorB.Copy());
        public EffectColorRandomUniform Copy() => new(ColorA.Copy(), ColorB.Copy());
    }
}