using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectAngleRandomPerComponent : IEffectAngle
    {
        [JsonProperty("aa")]
        public IFloat AngleA { get; set; }
        
        [JsonProperty("ab")]
        public IFloat AngleB { get; set; }
        
        public EffectAngleType GetModelType() => EffectAngleType.RandomPerComponent;

        public EffectAngleRandomPerComponent()
        {
            AngleA = new FloatValue(0f);
            AngleB = new FloatValue(0f);
        }
        public EffectAngleRandomPerComponent(float angleA, float angleB)
        {
            AngleA = new FloatValue(angleA);
            AngleB = new FloatValue(angleB);
        }
        public EffectAngleRandomPerComponent(IFloat angleA, IFloat angleB)
        {
            AngleA = angleA;
            AngleB = angleB;
        }
    }
}