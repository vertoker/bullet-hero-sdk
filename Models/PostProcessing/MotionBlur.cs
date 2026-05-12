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
    public class MotionBlur : Keyframe, // HEAVY IN ANY CASE, PHONES DON'T LIKE IT
        ICopyable<MotionBlur>, IEquatable<MotionBlur>
    {
        // Quality (client settings variable, he set it himself)
        
        [RuleInRange(PostProcessingRules.MotionBlur.IntensityMin,
            PostProcessingRules.MotionBlur.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }
        
        // Clamp (0.2f, predefined)

        public MotionBlur()
        {
            Intensity = 1f;
        }
        public MotionBlur(float intensity,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
        }

        public object Clone() => Copy();
        public MotionBlur Copy() => new(Intensity, Frame, Ease);

        public override bool Equals(object obj) => obj is MotionBlur value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Intensity);

        public bool Equals(MotionBlur other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Intensity.Equals(other.Intensity);
            return result;
        }
    }
}