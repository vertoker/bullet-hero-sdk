using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectColorRandomUniform : IEffectColor
    {
        [JsonProperty(ModelNames.ColorA)]
        public IColor ColorA { get; set; }
        
        [JsonProperty(ModelNames.ColorB)]
        public IColor ColorB { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.RandomUniform;

        public EffectColorRandomUniform()
        {
            ColorA = new ColorValue(EffectStatic.Color_ADefault);
            ColorB = new ColorValue(EffectStatic.Color_BDefault);
        }
        public EffectColorRandomUniform(Color colorA, Color colorB)
        {
            ColorA = new ColorValue(colorA);
            ColorB = new ColorValue(colorB);
        }
        public EffectColorRandomUniform(IColor colorA, IColor colorB)
        {
            ColorA = colorA;
            ColorB = colorB;
        }
    }
}