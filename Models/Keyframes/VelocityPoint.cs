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
    public class VelocityPoint : Keyframe, ICopyable<VelocityPoint>
    {
        [RuleNotNull(typeof(Vector2Value))]
        [JsonProperty(Names.Center)]
        public IVector2 Center { get; set; }
        
        [JsonProperty(Names.Force)]
        public float Force { get; set; }

        public VelocityPoint()
        {
            Center = new Vector2Circle();
            Force = 1;
        }
        public VelocityPoint(IVector2 center, float force, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Center = center;
            Force = force;
        }

        public object Clone() => Copy();
        public VelocityPoint Copy() => new(Center.Copy(), Force, Frame, Ease);
    }
}