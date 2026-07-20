using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.Keyframes
{
    [RuleContainer]
    public class BoolKey : IFrame, IModel<BoolKey>
    {
        [RuleLevelFrame]
        [JsonProperty(Names.FrameShort)]
        public int Frame { get; set; }
        
        [JsonProperty(Names.Bool)]
        public bool Value { get; set; }

        public BoolKey()
        {
            Frame = FrameRules.MinFrame;
            Value = false;
        }
        public BoolKey(bool value, int frame)
        {
            Frame = frame;
            Value = value;
        }
        public void Reset()
        {
            Frame = FrameRules.MinFrame;
            Value = false;
        }

        public object Clone() => Copy();
        public BoolKey Copy() => new(Value, Frame);

        public override bool Equals(object obj) => obj is BoolKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Frame, Value);

        public bool Equals(BoolKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Frame.Equals(other.Frame) && Value.Equals(other.Value);
            return result;
        }
    }
}