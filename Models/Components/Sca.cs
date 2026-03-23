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
        public IVector2 Vector2 { get; set; }

        public Sca()
        {
            Vector2 = new Vector2Circle();
        }
        public Sca(int frame, EaseType ease, IVector2 vector2) 
            : base(frame, ease)
        {
            Vector2 = vector2;
        }
    }
}