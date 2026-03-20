using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class Velocity : Keyframe
    {
        [JsonProperty("v")]
        public IVector Vector { get; set; }

        public Velocity()
        {
            Vector = new VectorCircle();
        }
        public Velocity(int frame, EaseType ease, IVector vector) 
            : base(frame, ease)
        {
            Vector = vector;
        }
    }
}