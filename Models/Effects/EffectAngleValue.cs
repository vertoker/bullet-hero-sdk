using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectAngleValue : IEffectAngle, ICopyable<EffectAngleValue>
    {
        [RuleNotNull]
        [JsonProperty(Names.Angle)]
        public IFloat Angle { get; set; }
        
        public EffectAngleType GetModelType() => EffectAngleType.Value;

        public EffectAngleValue()
        {
            Angle = new FloatValue(EffectRules.Angle.A_Default);
        }
        public EffectAngleValue(float angle)
        {
            Angle = new FloatValue(angle);
        }
        public EffectAngleValue(IFloat angle)
        {
            Angle = angle;
        }

        IEffectAngle ICopyable<IEffectAngle>.Copy() => new EffectAngleValue(Angle.Copy());
        public EffectAngleValue Copy() => new(Angle.Copy());
    }
}