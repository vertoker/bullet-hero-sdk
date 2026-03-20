using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class RotModel
    {
        [JsonProperty("f")]
        public int Frame { get; set; }
        
        [JsonProperty("a")]
        public IFloat Angle { get; set; }
        
        [JsonProperty("e")]
        public EaseType Ease { get; set; }

        public RotModel()
        {
            Frame = 0;
            Angle = new FloatValue();
            Ease = EaseType.Linear;
        }
        public RotModel(int frame, IFloat angle, EaseType ease)
        {
            Frame = frame;
            Angle = angle;
            Ease = ease;
        }
    }
}