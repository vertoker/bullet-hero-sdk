using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Values;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class Color2Key : Keyframe, ICopyable<Color2Key>, IEquatable<Color2Key>
    {
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.Color1)]
        public IColor Value1 { get; set; }
        
        [RuleNotNull(typeof(ColorValue))]
        [JsonProperty(Names.Color2)]
        public IColor Value2 { get; set; }

        public Color2Key()
        {
            Value1 = ColorValue.white;
            Value2 = ColorValue.white;
        }
        public Color2Key(IColor value, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Value1 = value.Copy();
            Value2 = value.Copy();
        }
        public Color2Key(IColor value1, IColor value2, int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public object Clone() => Copy();
        public Color2Key Copy() => new(Value1.Copy(), Value2.Copy(), Frame, Ease);

        public override bool Equals(object obj) => obj is Color2Key value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Value1, Value2);

        public bool Equals(Color2Key other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Value1.Equals(other.Value1)
                         && Value2.Equals(other.Value2);
            return result;
        }
    }
}