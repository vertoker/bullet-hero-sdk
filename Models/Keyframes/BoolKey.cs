using System;
using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Keyframes
{
    [RuleContainer]
    public class BoolKey : IFrame, ICopyable<BoolKey>, IEquatable<BoolKey>
    {
        [RuleLevelFrame]
        [JsonProperty(Names.FrameShort)]
        public int Frame { get; set; }
        
        [JsonProperty(Names.Bool)]
        public bool Value { get; set; }

        public BoolKey()
        {
            Frame = 0;
            
            Value = false;
        }
        public BoolKey(bool value, int frame)
        {
            Frame = frame;
            
            Value = value;
        }

        public object Clone() => Copy();
        public BoolKey Copy() => new(Value, Frame);

        public override bool Equals(object obj) => obj is BoolKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Frame, Value);

        public bool Equals(BoolKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Frame.Equals(other.Frame)
                         && Value.Equals(other.Value);
            return result;
        }
    }
}