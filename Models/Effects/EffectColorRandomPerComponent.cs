using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectColorRandomPerComponent : IEffectColor
    {
        [JsonProperty("ca")]
        public IColor ColorA { get; set; }
        
        [JsonProperty("cb")]
        public IColor ColorB { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.RandomPerComponent;

        public EffectColorRandomPerComponent()
        {
            ColorA = new ColorValue(Color.white);
            ColorB = new ColorValue(Color.white);
        }
        public EffectColorRandomPerComponent(Color colorA, Color colorB)
        {
            ColorA = new ColorValue(colorA);
            ColorB = new ColorValue(colorB);
        }
        public EffectColorRandomPerComponent(IColor colorA, IColor colorB)
        {
            ColorA = colorA;
            ColorB = colorB;
        }
    }
}