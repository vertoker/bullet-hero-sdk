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
        [RuleNotNull]
        [JsonProperty(Names.Color)]
        public IColor Value { get; set; }

        public Clr()
        {
            Value = ColorValue.white;
        }
        public Clr(int frame, EaseType ease, IColor value)
            : base(frame, ease)
        {
            Value = value;
        }

        public Clr Copy() => new(Frame, Ease, Value.Copy());
    }
}