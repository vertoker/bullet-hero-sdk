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
    public class ZoomKey : Keyframe, ICopyable<ZoomKey>, IEquatable<ZoomKey>
    {
        [RuleNotNull(typeof(FloatValue)), RuleIFloatInRange(ValueRules.MinZoom, ValueRules.MaxZoom)]
        [JsonProperty(Names.Float)]
        public IFloat Zoom { get; set; }

        public ZoomKey()
        {
            Zoom = new FloatValue();
        }
        public ZoomKey(IFloat zoom, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Zoom = zoom;
        }

        public object Clone() => Copy();
        public ZoomKey Copy() => new(Zoom.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is ZoomKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Zoom);

        public bool Equals(ZoomKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other) && Zoom.Equals(other.Zoom);
            return result;
        }
    }
}