using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values.Vectors;
using BHSDK.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectScaleRandomPerComponent : IEffectScale, ICopyable<EffectScaleRandomPerComponent>
    {
        [JsonProperty(Names.ScaleX)]
        public IVector2 ScaleA { get; set; }
        
        [JsonProperty(Names.ScaleY)]
        public IVector2 ScaleB { get; set; }
        
        public EffectScaleType GetModelType() => EffectScaleType.RandomPerComponent;

        public EffectScaleRandomPerComponent()
        {
            ScaleA = new Vector2Value(EffectStatic.Scale_ADefault);
            ScaleB = new Vector2Value(EffectStatic.Scale_BDefault);
        }
        public EffectScaleRandomPerComponent(Vector2 scaleA, Vector2 scaleB)
        {
            ScaleA = new Vector2Value(scaleA);
            ScaleB = new Vector2Value(scaleB);
        }
        public EffectScaleRandomPerComponent(IVector2 scaleA, IVector2 scaleB)
        {
            ScaleA = scaleA;
            ScaleB = scaleB;
        }

        IEffectScale ICopyable<IEffectScale>.Copy() => new EffectScaleRandomPerComponent(ScaleA.Copy(), ScaleB.Copy());
        public EffectScaleRandomPerComponent Copy() => new(ScaleA.Copy(), ScaleB.Copy());
    }
}