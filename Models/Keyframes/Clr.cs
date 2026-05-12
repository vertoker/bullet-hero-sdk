using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    [RuleContainer]
    public class Clr : Keyframe, ICopyable<Clr>
    {
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.Color)]
        public IColor Value { get; set; }

        public Clr()
        {
            Value = ColorValue.white;
        }
        public Clr(IColor value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Value = value;
        }

        public object Clone() => Copy();
        public Clr Copy() => new(Value.Copy(), Frame, Ease);
    }
}