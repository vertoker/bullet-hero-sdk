using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectColorRandomUniform : IEffectColor, ICopyable<EffectColorRandomUniform>
    {
        [JsonProperty(Names.ColorA)]
        public IColor ColorA { get; set; }
        
        [JsonProperty(Names.ColorB)]
        public IColor ColorB { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.RandomUniform;

        public EffectColorRandomUniform()
        {
            ColorA = new ColorValue(
                EffectStatic.Color_A_R_Default,
                EffectStatic.Color_A_G_Default,
                EffectStatic.Color_A_B_Default,
                EffectStatic.Color_A_A_Default);
            ColorB = new ColorValue(
                EffectStatic.Color_B_R_Default,
                EffectStatic.Color_B_G_Default,
                EffectStatic.Color_B_B_Default,
                EffectStatic.Color_B_A_Default);
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