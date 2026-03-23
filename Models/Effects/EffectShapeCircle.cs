using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectShapeCircle : IEffectShape
    {
        // [JsonProperty("s")]
        // public IEffectShapeSpread Spread { get; set; }
        
        public EffectShapeType Type => EffectShapeType.Circle;
    }
}