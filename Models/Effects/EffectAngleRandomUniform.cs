using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectAngleRandomUniform : IEffectAngle
    {
        [JsonProperty(ModelNames.AngleA)]
        public IFloat AngleA { get; set; }
        
        [JsonProperty(ModelNames.AngleB)]
        public IFloat AngleB { get; set; }

        public EffectAngleType GetModelType() => EffectAngleType.RandomUniform;

        public EffectAngleRandomUniform()
        {
            AngleA = new FloatValue(0f);
            AngleB = new FloatValue(0f);
        }
        public EffectAngleRandomUniform(float angleA, float angleB)
        {
            AngleA = new FloatValue(angleA);
            AngleB = new FloatValue(angleB);
        }
        public EffectAngleRandomUniform(IFloat angleA, IFloat angleB)
        {
            AngleA = angleA;
            AngleB = angleB;
        }
    }
}