using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class Pos : Keyframe
    {
        [JsonProperty(Names.Vector2)]
        public IVector2 Vector2 { get; set; }
        
        [JsonProperty(Names.Anchor)]
        public Alignment Anchor { get; set; }

        public Pos()
        {
            Vector2 = new Vector2Value();
            Anchor = Alignment.MiddleCenter;
        }
        public Pos(int frame, EaseType ease, IVector2 vector2, Alignment anchor) 
            : base(frame, ease)
        {
            Vector2 = vector2;
            Anchor = anchor;
        }
    }
}