using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.Events
{
    [RuleContainer]
    public class ThemeKeyframe : Keyframe
    {
        [RuleThemeIndex]
        [JsonProperty(Names.ThemeIndex)]
        public int ThemeIndex { get; set; } // reference to all level Themes list

        public ThemeKeyframe()
        {
            ThemeIndex = 0;
        }
        public ThemeKeyframe(int themeIndex, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            ThemeIndex = themeIndex;
        }
    }
}