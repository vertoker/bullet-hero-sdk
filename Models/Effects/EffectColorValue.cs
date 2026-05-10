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
    public class EffectColorValue : IEffectColor, ICopyable<EffectColorValue>
    {
        [RuleNotNull]
        [JsonProperty(Names.Color)]
        public IColor Color { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.Value;

        public EffectColorValue()
        {
            Color = new ColorValue(
                EffectRules.Color.A_R_Default,
                EffectRules.Color.A_G_Default,
                EffectRules.Color.A_B_Default,
                EffectRules.Color.A_A_Default);
        }
        public EffectColorValue(IColor color)
        {
            Color = color;
        }

        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorValue(Color.Copy());
        public EffectColorValue Copy() => new(Color.Copy());
    }
}