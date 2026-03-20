using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class VelocityPoint : Keyframe
    {
        [JsonProperty("c")]
        public IVector Center { get; set; }
        
        [JsonProperty("f")]
        public float Force { get; set; }

        public VelocityPoint()
        {
            Center = new VectorCircle();
            Force = 1;
        }
        public VelocityPoint(int frame, EaseType ease, 
            IVector center, float force)
            : base(frame, ease)
        {
            Center = center;
            Force = force;
        }
    }
}