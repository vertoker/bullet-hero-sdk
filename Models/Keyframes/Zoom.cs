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
    public class Zoom : Keyframe, ICopyable<Zoom>
    {
        [RuleNotNull(typeof(FloatValue)), RuleIFloatInRange(ValueRules.MinZoom, ValueRules.MaxZoom)]
        [JsonProperty(Names.Size)]
        public IFloat Size { get; set; }

        public Zoom()
        {
            Size = new FloatValue();
        }
        public Zoom(IFloat size, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Size = size;
        }

        public object Clone() => Copy();
        public Zoom Copy() => new(Size.Copy(), Frame, Ease);
    }
}