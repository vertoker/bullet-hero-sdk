using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectColorValue : IEffectColor
    {
        [JsonProperty(ModelNames.Color)]
        public IColor Color { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.Value;

        public EffectColorValue()
        {
            Color = new ColorValue(EffectStatic.ColorADefault);
        }
        public EffectColorValue(Color color)
        {
            Color = new ColorValue(color);
        }
        public EffectColorValue(IColor color)
        {
            Color = color;
        }
    }
}