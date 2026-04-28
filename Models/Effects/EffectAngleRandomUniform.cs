using BHSDK.Models.Enum.Effects;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Effects;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.Effects
{
    public class EffectAngleRandomUniform : IEffectAngle, ICopyable<EffectAngleRandomUniform>
    {
        [JsonProperty(Names.AngleA)]
        public IFloat AngleA { get; set; }
        
        [JsonProperty(Names.AngleB)]
        public IFloat AngleB { get; set; }

        public EffectAngleType GetModelType() => EffectAngleType.RandomUniform;

        public EffectAngleRandomUniform()
        {
            AngleA = new FloatValue(EffectStatic.Angle_ADefault);
            AngleB = new FloatValue(EffectStatic.Angle_BDefault);
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

        IEffectAngle ICopyable<IEffectAngle>.Copy() => new EffectAngleRandomUniform(AngleA.Copy(), AngleB.Copy());
        public EffectAngleRandomUniform Copy() => new(AngleA.Copy(), AngleB.Copy());
    }
}