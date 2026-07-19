using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Primitives;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Events
{
    [RuleContainer]
    public class ThemeKeyframe : Keyframe, ICopyable<ThemeKeyframe>, IEquatable<ThemeKeyframe>
    {
        [JsonProperty(Names.ThemeId)]
        public ThemeId ThemeId { get; set; }

        public ThemeKeyframe()
        {
            ThemeId = ThemeId.Null;
        }
        public ThemeKeyframe(ThemeId themeId, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            ThemeId = themeId;
        }

        public object Clone() => Copy();
        public ThemeKeyframe Copy() => new(ThemeId, Frame, Ease);

        public bool Equals(ThemeKeyframe other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && ThemeId.Equals(other.ThemeId);
            return result;
        }

        public override bool Equals(object obj) => obj is ThemeKeyframe value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), ThemeId);
    }
}
