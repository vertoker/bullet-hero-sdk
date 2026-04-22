using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    // TODO activate for player when add events
    public class VelocityPoint : Keyframe
    {
        [JsonProperty(Names.Center)]
        public IVector2 Center { get; set; }
        
        [JsonProperty(Names.Force)]
        public float Force { get; set; }

        public VelocityPoint()
        {
            Center = new Vector2Circle();
            Force = 1;
        }
        public VelocityPoint(int frame, EaseType ease, 
            IVector2 center, float force)
            : base(frame, ease)
        {
            Center = center;
            Force = force;
        }
    }
}