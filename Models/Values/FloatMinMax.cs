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
    public class FloatMinMax : IFloat, ICopyable<FloatMinMax>, IEquatable<FloatMinMax>
    {
        [JsonProperty(Names.Min)]
        public float Min { get; set; }
        
        [JsonProperty(Names.Max)]
        public float Max { get; set; }

        public FloatMinMax()
        {
            Min = 0f;
            Max = 1f;
        }
        public FloatMinMax(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public FloatType GetModelType() => FloatType.RandomMinMax;
        
        public object Clone() => Copy();
        IFloat ICopyable<IFloat>.Copy() => new FloatMinMax(Min, Max);
        public FloatMinMax Copy() => new(Min, Max);

        public override bool Equals(object obj) => obj is FloatMinMax value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Min, Max);
        
        public bool Equals(IFloat other) => other is FloatMinMax value && Equals(value);
        public bool Equals(FloatMinMax other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Min.Equals(other.Min)
                         && Max.Equals(other.Max);
            return result;
        }
    }
}