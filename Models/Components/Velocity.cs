using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    // TODO activate for player when add events
    public class Velocity : Keyframe
    {
        [JsonProperty(ModelNames.Vector2)]
        public IVector2 Vector2 { get; set; }

        public Velocity()
        {
            Vector2 = new Vector2Circle();
        }
        public Velocity(int frame, EaseType ease, IVector2 vector2) 
            : base(frame, ease)
        {
            Vector2 = vector2;
        }
    }
}