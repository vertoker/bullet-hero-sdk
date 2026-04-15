using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectAngleValue : IEffectAngle
    {
        [JsonProperty(ModelNames.Angle)]
        public IFloat Angle { get; set; }
        
        public EffectAngleType GetModelType() => EffectAngleType.Value;

        public EffectAngleValue()
        {
            Angle = new FloatValue(EffectStatic.Angle_ADefault);
        }
        public EffectAngleValue(float angle)
        {
            Angle = new FloatValue(angle);
        }
        public EffectAngleValue(IFloat angle)
        {
            Angle = angle;
        }
    }
}