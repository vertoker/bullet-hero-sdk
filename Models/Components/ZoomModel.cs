using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class ZoomModel
    {
        [JsonProperty("f")]
        public int Frame { get; set; }
        
        [JsonProperty("s")]
        public IFloat Size { get; set; }
        
        [JsonProperty("e")]
        public EaseType Ease { get; set; }

        public ZoomModel()
        {
            Frame = 0;
            Size = new FloatValue();
            Ease = EaseType.Linear;
        }
        public ZoomModel(int frame, IFloat size, EaseType ease)
        {
            Frame = frame;
            Size = size;
            Ease = ease;
        }
    }
}