using System;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class WhiteBalance : Keyframe,
        ICopyable<WhiteBalance>, IEquatable<WhiteBalance>
    {
        [RuleInRange(PostProcessingRules.WhiteBalance.TemperatureMin,
            PostProcessingRules.WhiteBalance.TemperatureMax)]
        [JsonProperty(Names.Temperature)]
        public float Temperature { get; set; }
        
        [RuleInRange(PostProcessingRules.WhiteBalance.TintMin,
            PostProcessingRules.WhiteBalance.TintMax)]
        [JsonProperty(Names.Tint)]
        public float Tint { get; set; }

        public WhiteBalance()
        {
            Temperature = 0f;
            Tint = 0f;
        }
        public WhiteBalance(float temperature, float tint,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Temperature = temperature;
            Tint = tint;
        }

        public object Clone() => Copy();
        public WhiteBalance Copy() => new(Temperature, Tint, Frame, Ease);

        public override bool Equals(object obj) => obj is WhiteBalance value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Temperature, Tint);

        public bool Equals(WhiteBalance other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Temperature.Equals(other.Temperature)
                         && Tint.Equals(other.Tint);
            return result;
        }
    }
}