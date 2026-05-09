using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectColorValue : IEffectColor, ICopyable<EffectColorValue>
    {
        [JsonProperty(Names.Color)]
        public IColor Color { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.Value;

        public EffectColorValue()
        {
            Color = new ColorValue(
                EffectStatic.Color_A_R_Default,
                EffectStatic.Color_A_G_Default,
                EffectStatic.Color_A_B_Default,
                EffectStatic.Color_A_A_Default);
        }
        public EffectColorValue(IColor color)
        {
            Color = color;
        }

        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorValue(Color.Copy());
        public EffectColorValue Copy() => new(Color.Copy());
    }
}