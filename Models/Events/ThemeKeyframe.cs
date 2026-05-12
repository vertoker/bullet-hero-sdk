using System;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Keyframes;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Events
{
    [RuleContainer]
    public class ThemeKeyframe : Keyframe, ICopyable<ThemeKeyframe>, IEquatable<ThemeKeyframe>
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

        public object Clone() => Copy();
        public ThemeKeyframe Copy() => new(ThemeIndex, Frame, Ease);

        public bool Equals(ThemeKeyframe other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && ThemeIndex.Equals(other.ThemeIndex);
            return result;
        }

        public override bool Equals(object obj) => obj is ThemeKeyframe value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), ThemeIndex);
    }
}