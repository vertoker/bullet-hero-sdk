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
    public class Sca : Keyframe, ICopyable<Sca>
    {
        [RuleNotNull(typeof(Vector2Value)), RuleIVector2InRange(ValueRules.MinSca, ValueRules.MaxSca)]
        [JsonProperty(Names.Vector2)]
        public IVector2 Vector2 { get; set; }

        public Sca()
        {
            Vector2 = new Vector2Circle();
        }
        public Sca(IVector2 vector2, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Vector2 = vector2;
        }

        public object Clone() => Copy();
        public Sca Copy() => new(Vector2.Copy(), Frame, Ease);
    }
}