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
    public class FloatMinMaxStep : IFloat, ICopyable<FloatMinMaxStep>, IEquatable<FloatMinMaxStep>
    {
        [JsonProperty(Names.Min)]
        public float Min { get; set; }
        
        [JsonProperty(Names.Max)]
        public float Max { get; set; }
        
        [RuleMin(0f)]
        [JsonProperty(Names.Step)]
        public float Step { get; set; }

        public FloatMinMaxStep()
        {
            Min = 0f;
            Max = 1f;
            Step = 1f;
        }
        public FloatMinMaxStep(float min, float max, float step)
        {
            Min = min;
            Max = max;
            Step = step;
        }

        public FloatType GetModelType() => FloatType.RandomMinMaxStep;

        public object Clone() => Copy();
        IFloat ICopyable<IFloat>.Copy() => new FloatMinMaxStep(Min, Max, Step);
        public FloatMinMaxStep Copy() => new(Min, Max, Step);

        public override bool Equals(object obj) => obj is FloatMinMaxStep value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(Min, Max, Step);
        
        public bool Equals(IFloat other) => other is FloatMinMaxStep value && Equals(value);
        public bool Equals(FloatMinMaxStep other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = Min.Equals(other.Min)
                         && Max.Equals(other.Max)
                         && Step.Equals(other.Step);
            return result;
        }
    }
}