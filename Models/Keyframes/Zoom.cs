using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class Zoom : Keyframe
    {
        [JsonProperty(ModelNames.Size)]
        public IFloat Size { get; set; }

        public Zoom()
        {
            Size = new FloatValue();
        }
        public Zoom(int frame, EaseType ease, IFloat size)
            : base(frame, ease)
        {
            Size = size;
        }
    }
}