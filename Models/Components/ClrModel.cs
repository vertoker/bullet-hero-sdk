using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class ClrModel
    {
        [JsonProperty("f")]
        public int Frame { get; set; }
        
        [JsonProperty("c")]
        public IColor Color { get; set; }
        
        [JsonProperty("e")]
        public EaseType Ease { get; set; }

        public ClrModel()
        {
            Frame = 0;
            Color = new ColorValue(1f, 1f, 1f, 1f);
            Ease = EaseType.Linear;
        }
        public ClrModel(int frame, IColor color, EaseType ease)
        {
            Frame = frame;
            Color = color;
            Ease = ease;
        }
    }
}