using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    [RuleContainer]
    public class IntKey : Keyframe, ICopyable<IntKey>
    {
        [RuleNotNull(typeof(IntValue))]
        [JsonProperty(Names.Int)]
        public IInt Value { get; set; }

        public IntKey()
        {
            Value = new IntValue(0);
        }
        public IntKey(int frame, EaseType ease, IInt value)
            : base(frame, ease)
        {
            Value = value;
        }

        public IntKey Copy() => new(Frame, Ease, Value.Copy());
    }
}