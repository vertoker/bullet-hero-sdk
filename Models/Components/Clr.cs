using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Components
{
    public class Clr : Keyframe
    {
        [JsonProperty(ModelNames.Color)]
        public IColor Color { get; set; }

        public Clr()
        {
            Color = new ColorValue(UnityEngine.Color.white);
        }
        public Clr(int frame, EaseType ease, IColor color)
            : base(frame, ease)
        {
            Color = color;
        }
    }
}