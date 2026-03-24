using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using Newtonsoft.Json;

namespace BHSDK.Models.Events
{
    public class ThemeKeyframe : Keyframe
    {
        [JsonProperty(ModelNames.Theme + ModelNames.Index)]
        public int ThemeIndex { get; set; }

        public ThemeKeyframe()
        {
            ThemeIndex = 0;
        }
        public ThemeKeyframe(int frame, EaseType ease, int themeIndex) : base(frame, ease)
        {
            ThemeIndex = themeIndex;
        }
    }
}