using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class Sca : Keyframe
    {
        [JsonProperty("v")]
        public IVector Vector { get; set; }

        public Sca()
        {
            Vector = new VectorCircle();
        }
        public Sca(int frame, EaseType ease, IVector vector) 
            : base(frame, ease)
        {
            Vector = vector;
        }
    }
}