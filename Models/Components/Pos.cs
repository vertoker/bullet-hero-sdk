using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class Pos : Keyframe
    {
        [JsonProperty(ModelNames.Vector2)]
        public IVector2 Vector2 { get; set; }
        
        [JsonProperty(ModelNames.Anchor)]
        public Anchor Anchor { get; set; }

        public Pos()
        {
            Vector2 = new Vector2Value();
            Anchor = Anchor.Center_Middle;
        }
        public Pos(int frame, EaseType ease, IVector2 vector2, Anchor anchor) 
            : base(frame, ease)
        {
            Vector2 = vector2;
            Anchor = anchor;
        }
    }
}