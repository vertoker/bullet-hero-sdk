using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class ScreenLimitKey : Keyframe, ICopyable<ScreenLimitKey>, IEquatable<ScreenLimitKey>
    {
        // Same default-fix target as GameEvents.ScreenLimit (the single-value predecessor this
        // keyframe track replaces) - mappers choose limitations for the screen themselves.
        [RuleNotNull(typeof(ScreenLimitFixed))]
        [JsonProperty(Names.ScreenLimit)]
        public IScreenLimit ScreenLimit { get; set; }

        public ScreenLimitKey()
        {
            ScreenLimit = new ScreenLimitNone();
        }
        public ScreenLimitKey(IScreenLimit screenLimit, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            ScreenLimit = screenLimit;
        }

        public object Clone() => Copy();
        public ScreenLimitKey Copy() => new(ScreenLimit.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is ScreenLimitKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), ScreenLimit);

        public bool Equals(ScreenLimitKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other) && ScreenLimit.Equals(other.ScreenLimit);
            return result;
        }
    }
}
