using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using Newtonsoft.Json;

namespace BHSDK.Models.Events
{
    public class ThemeKeyframe : Keyframe
    {
        [JsonProperty(Names.ThemeIndex)]
        public int ThemeIndex { get; set; } // reference to all level Themes list

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