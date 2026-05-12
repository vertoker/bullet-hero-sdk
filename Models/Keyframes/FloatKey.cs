using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    [RuleContainer]
    public class FloatKey : Keyframe, ICopyable<FloatKey>
    {
        [RuleNotNull(typeof(FloatValue))]
        [JsonProperty(Names.Float)]
        public IFloat Value { get; set; }

        public FloatKey()
        {
            Value = new FloatValue(0f);
        }
        public FloatKey(IFloat value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Value = value;
        }

        public object Clone() => Copy();
        public FloatKey Copy() => new(Value.Copy(), Frame, Ease);
    }
}