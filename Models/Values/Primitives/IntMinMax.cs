using System;
using BHSDK.Models.Enum.Values;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.Values
{
    [RuleContainer]
    public class IntMinMax : IInt, ICopyable<IntMinMax>, IEquatable<IntMinMax>
    {
        [JsonProperty(Names.Min)]
        public int Min { get; set; }
        
        [JsonProperty(Names.Max)]
        public int Max { get; set; }

        public IntMinMax()
        {
            Min = 0;
            Max = 1;
        }
        public IntMinMax(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public IntType GetModelType() => IntType.RandomMinMax;
        
        public object Clone() => Copy();
        IInt ICopyable<IInt>.Copy() => new IntMinMax(Min, Max);
        public IntMinMax Copy() => new(Min, Max);

        public override bool Equals(object obj) => obj is IntMinMax value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Min, Max);
        
        public bool Equals(IInt other) => other is IntMinMax value && Equals(value);
        public bool Equals(IntMinMax other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Min.Equals(other.Min)
                         && Max.Equals(other.Max);
            return result;
        }
    }
}