using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectAngleRandomPerComponent : IEffectAngle, ICopyable<EffectAngleRandomPerComponent>
    {
        [JsonProperty(Names.AngleA)]
        public IFloat AngleA { get; set; }
        
        [JsonProperty(Names.AngleB)]
        public IFloat AngleB { get; set; }
        
        public EffectAngleType GetModelType() => EffectAngleType.RandomPerComponent;

        public EffectAngleRandomPerComponent()
        {
            AngleA = new FloatValue(EffectStatic.Angle_ADefault);
            AngleB = new FloatValue(EffectStatic.Angle_BDefault);
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

        IEffectAngle ICopyable<IEffectAngle>.Copy() => new EffectAngleRandomPerComponent(AngleA.Copy(), AngleB.Copy());
        public EffectAngleRandomPerComponent Copy() => new(AngleA.Copy(), AngleB.Copy());
    }
}