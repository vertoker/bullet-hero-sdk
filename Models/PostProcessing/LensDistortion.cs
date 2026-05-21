using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Interfaces.Values;
using BH.SDK.Models.Keyframes;
using BH.SDK.Models.Values;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.PostProcessing
{
    [RuleContainer]
    public class LensDistortion : Keyframe,
        ICopyable<LensDistortion>, IEquatable<LensDistortion>
    {
        [RuleInRange(PostProcessingRules.LensDistortion.IntensityMin,
            PostProcessingRules.LensDistortion.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }
        
        [RuleNotNull(typeof(Vector2Value)), RuleIVector2InRange(PostProcessingRules.LensDistortion.MultiplierMin,
             PostProcessingRules.LensDistortion.MultiplierMax)]
        [JsonProperty(Names.Multiplier)]
        public IVector2 Multiplier { get; set; }
        
        [RuleNotNull(typeof(Vector2Value)), RuleIVector2InRange(PostProcessingRules.LensDistortion.CenterMin,
             PostProcessingRules.LensDistortion.CenterMax)]
        [JsonProperty(Names.Center)]
        public IVector2 Center { get; set; }
        
        [RuleInRange(PostProcessingRules.LensDistortion.ScaleMin,
            PostProcessingRules.LensDistortion.ScaleMax)]
        [JsonProperty(Names.Scale)]
        public float Scale { get; set; }

        public LensDistortion()
        {
            Intensity = 0.5f;
            Multiplier = new Vector2Value(1f, 1f);
            Center = new Vector2Value(0.5f, 0.5f);
            Scale = 1f;
        }
        public LensDistortion(float intensity, IVector2 multiplier, IVector2 center, float scale,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Intensity = intensity;
            Multiplier = multiplier;
            Center = center;
            Scale = scale;
        }

        public object Clone() => Copy();
        public LensDistortion Copy() => new(Intensity, Multiplier.Copy(), Center.Copy(), Scale, Frame, Ease);

        public override bool Equals(object obj) => obj is LensDistortion value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Intensity, Multiplier, Center, Scale);

        public bool Equals(LensDistortion other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Intensity.Equals(other.Intensity)
                         && Multiplier.Equals(other.Multiplier)
                         && Center.Equals(other.Center)
                         && Scale.Equals(other.Scale);
            return result;
        }
    }
}