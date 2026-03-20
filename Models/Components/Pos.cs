using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class Pos : Keyframe
    {
        [JsonProperty("v")]
        public IVector Vector { get; set; }
        
        [JsonProperty("a")]
        public Anchor Anchor { get; set; }

        public Pos()
        {
            Vector = new VectorValue();
            Anchor = Anchor.Center_Middle;
        }
        public Pos(int frame, EaseType ease, IVector vector, Anchor anchor) 
            : base(frame, ease)
        {
            Vector = vector;
            Anchor = anchor;
        }
    }
}