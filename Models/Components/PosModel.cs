using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class PosModel
    {
        [JsonProperty("f")]
        public int Frame { get; set; }
        
        [JsonProperty("v")]
        public IVector Vector { get; set; }
        
        [JsonProperty("a")]
        public Anchor Anchor { get; set; }
        
        [JsonProperty("e")]
        public EaseType Ease { get; set; }

        public PosModel()
        {
            Frame = 0;
            Vector = new VectorValue();
            Anchor = Anchor.Center_Middle;
            Ease = EaseType.Linear;
        }
        public PosModel(int frame, IVector vector, Anchor anchor, EaseType ease)
        {
            Frame = frame;
            Vector = vector;
            Anchor = anchor;
            Ease = ease;
        }
    }
}