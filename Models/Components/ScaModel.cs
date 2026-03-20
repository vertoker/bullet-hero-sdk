using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class ScaModel
    {
        [JsonProperty("f")]
        public int Frame { get; set; }
        
        [JsonProperty("v")]
        public IVector Vector { get; set; }
        
        [JsonProperty("e")]
        public EaseType Ease { get; set; }

        public ScaModel()
        {
            Frame = 0;
            Vector = new VectorCircle();
            Ease = EaseType.Linear;
        }
        public ScaModel(int frame, IVector vector, EaseType ease)
        {
            Frame = frame;
            Vector = vector;
            Ease = ease;
        }
    }
}