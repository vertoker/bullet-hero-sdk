using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectScaleRandomUniform : IEffectScale
    {
        [JsonProperty(Names.ScaleX)]
        public IVector2 ScaleA { get; set; }
        
        [JsonProperty(Names.ScaleY)]
        public IVector2 ScaleB { get; set; }

        public EffectScaleType GetModelType() => EffectScaleType.RandomUniform;

        public EffectScaleRandomUniform()
        {
            ScaleA = new Vector2Value(EffectStatic.Scale_ADefault);
            ScaleB = new Vector2Value(EffectStatic.Scale_BDefault);
        }
        public EffectScaleRandomUniform(Vector2 scaleA, Vector2 scaleB)
        {
            ScaleA = new Vector2Value(scaleA);
            ScaleB = new Vector2Value(scaleB);
        }
        public EffectScaleRandomUniform(IVector2 scaleA, IVector2 scaleB)
        {
            ScaleA = scaleA;
            ScaleB = scaleB;
        }
    }
}