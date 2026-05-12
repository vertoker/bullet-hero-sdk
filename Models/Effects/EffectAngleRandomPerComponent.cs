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
    [RuleContainer]
    public class EffectAngleRandomPerComponent : IEffectAngle, ICopyable<EffectAngleRandomPerComponent>
    {
        [RuleNotNull]
        [JsonProperty(Names.AngleA)]
        public IFloat AngleA { get; set; }
        
        [RuleNotNull]
        [JsonProperty(Names.AngleB)]
        public IFloat AngleB { get; set; }
        
        public EffectAngleType GetModelType() => EffectAngleType.RandomPerComponent;

        public EffectAngleRandomPerComponent()
        {
            AngleA = new FloatValue(EffectRules.Angle.A_Default);
            AngleB = new FloatValue(EffectRules.Angle.B_Default);
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

        public object Clone() => Copy();
        IEffectAngle ICopyable<IEffectAngle>.Copy() => new EffectAngleRandomPerComponent(AngleA.Copy(), AngleB.Copy());
        public EffectAngleRandomPerComponent Copy() => new(AngleA.Copy(), AngleB.Copy());
    }
}