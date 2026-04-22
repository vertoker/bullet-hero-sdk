using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Effects
{
    public class EffectScaleValue : IEffectScale
    {
        [JsonProperty(Names.Scale)]
        public IVector2 Scale { get; set; }
        
        public EffectScaleType GetModelType() => EffectScaleType.Value;

        public EffectScaleValue()
        {
            Scale = new Vector2Value(EffectStatic.Scale_ADefault);
        }
        public EffectScaleValue(Vector2 scale)
        {
            Scale = new Vector2Value(scale);
        }
        public EffectScaleValue(IVector2 scale)
        {
            Scale = scale;
        }
    }
}