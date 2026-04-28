using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectColorValue : IEffectColor, ICopyable<EffectColorValue>
    {
        [JsonProperty(Names.Color)]
        public IColor Color { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.Value;

        public EffectColorValue()
        {
            Color = new ColorValue(EffectStatic.Color_ADefault);
        }
        public EffectColorValue(Color color)
        {
            Color = new ColorValue(color);
        }
        public EffectColorValue(IColor color)
        {
            Color = color;
        }

        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorValue(Color.Copy());
        public EffectColorValue Copy() => new(Color.Copy());
    }
}