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
    public class EffectColorRandomPerComponent : IEffectColor, ICopyable<EffectColorRandomPerComponent>
    {
        [JsonProperty(Names.ColorA)]
        public IColor ColorA { get; set; }
        
        [JsonProperty(Names.ColorB)]
        public IColor ColorB { get; set; }
        
        public EffectColorType GetModelType() => EffectColorType.RandomPerComponent;

        public EffectColorRandomPerComponent()
        {
            ColorA = new ColorValue(EffectStatic.Color_ADefault);
            ColorB = new ColorValue(EffectStatic.Color_BDefault);
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

        IEffectColor ICopyable<IEffectColor>.Copy() => new EffectColorRandomPerComponent(ColorA.Copy(), ColorB.Copy());
        public EffectColorRandomPerComponent Copy() => new(ColorA.Copy(), ColorB.Copy());
    }
}