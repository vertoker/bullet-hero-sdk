using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class AlignmentKey : Keyframe, ICopyable<AlignmentKey>, IEquatable<AlignmentKey>
    {
        [RuleNotNull(typeof(Vector2Value)), RuleIVector2InRange(ValueRules.MinAlignment, ValueRules.MaxAlignment)]
        [JsonProperty(Names.Vector2)]
        public IVector2 Value { get; set; }

        public AlignmentKey()
        {
            Value = Alignment.DefaultValue;
        }
        public AlignmentKey(IVector2 value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Value = value;
        }

        public object Clone() => Copy();
        public AlignmentKey Copy() => new(Value.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is AlignmentKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Frame, Value);

        public bool Equals(AlignmentKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other) && Value.Equals(other.Value);
            return result;
        }
        
        public static AlignmentKey GetLeftBottom(int frame, EaseType ease = DefaultEase)
            => new(Alignment.LeftBottomValue, frame, ease);
        public static AlignmentKey GetLeftMiddle(int frame, EaseType ease = DefaultEase)
            => new(Alignment.LeftMiddleValue, frame, ease);
        public static AlignmentKey GetLeftTop(int frame, EaseType ease = DefaultEase)
            => new(Alignment.LeftTopValue, frame, ease);
        public static AlignmentKey GetCenterBottom(int frame, EaseType ease = DefaultEase)
            => new(Alignment.CenterBottomValue, frame, ease);
        public static AlignmentKey GetCenterMiddle(int frame, EaseType ease = DefaultEase)
            => new(Alignment.CenterMiddleValue, frame, ease);
        public static AlignmentKey GetCenterTop(int frame, EaseType ease = DefaultEase)
            => new(Alignment.CenterTopValue, frame, ease);
        public static AlignmentKey GetRightBottom(int frame, EaseType ease = DefaultEase)
            => new(Alignment.RightBottomValue, frame, ease);
        public static AlignmentKey GetRightMiddle(int frame, EaseType ease = DefaultEase)
            => new(Alignment.RightMiddleValue, frame, ease);
        public static AlignmentKey GetRightTop(int frame, EaseType ease = DefaultEase)
            => new(Alignment.RightTopValue, frame, ease);
    }
}