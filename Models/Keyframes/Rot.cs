using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    [RuleContainer]
    public class Rot : Keyframe, ICopyable<Rot>
    {
        [RuleNotNull(typeof(FloatValue)), RuleIFloatInRange(ValueRules.MinCoord, ValueRules.MaxCoord)]
        [JsonProperty(Names.Angle)]
        public IFloat Angle { get; set; }

        public Rot()
        {
            Angle = new FloatValue();
        }
        public Rot(IFloat angle, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Angle = angle;
        }

        public object Clone() => Copy();
        public Rot Copy() => new(Angle.Copy(), Frame, Ease);
    }
}