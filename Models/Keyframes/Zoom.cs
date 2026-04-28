using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Values;
using Newtonsoft.Json;

namespace BHSDK.Models.Keyframes
{
    public class Zoom : Keyframe, ICopyable<Zoom>
    {
        [JsonProperty(Names.Size)]
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

        public Zoom Copy() => new(Frame, Ease, Size.Copy());
    }
}