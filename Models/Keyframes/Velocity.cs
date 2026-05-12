using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    // TODO activate for player when add events
    [RuleContainer]
    public class Velocity : Keyframe, ICopyable<Velocity>
    {
        [RuleNotNull(typeof(Vector2Value))]
        [JsonProperty(Names.Vector2)]
        public IVector2 Vector2 { get; set; }

        public Velocity()
        {
            Vector2 = new Vector2Circle();
        }
        public Velocity(IVector2 vector2, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Vector2 = vector2;
        }

        public Velocity Copy() => new(Vector2.Copy(), Frame, Ease);
    }
}