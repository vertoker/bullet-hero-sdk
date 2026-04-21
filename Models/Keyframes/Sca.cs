using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class Sca : Keyframe
    {
        [JsonProperty(ModelNames.Vector2)]
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