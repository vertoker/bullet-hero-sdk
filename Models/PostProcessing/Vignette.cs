using System;
using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Interfaces.Values;
using BHSDK.Models.Keyframes;
using BHSDK.Models.Values;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class Vignette : Keyframe,
        ICopyable<Vignette>, IEquatable<Vignette>
    {
        [RuleNotNull(typeof(ColorValue))] // TODO add extra part for checking HDR part
        [JsonProperty(Names.Color)]
        public IColor ColorHDR { get; set; }
        
        [RuleNotNull(typeof(Vector2Value)), RuleIVector2InRange(PostProcessingRules.Vignette.CenterMin,
             PostProcessingRules.Vignette.CenterMax)]
        [JsonProperty(Names.Center)]
        public IVector2 Center { get; set; }
        
        [RuleInRange(PostProcessingRules.Vignette.IntensityMin,
             PostProcessingRules.Vignette.IntensityMax)]
        [JsonProperty(Names.Intensity)]
        public float Intensity { get; set; }
        
        [RuleInRange(PostProcessingRules.Vignette.SmoothnessMin,
            PostProcessingRules.Vignette.SmoothnessMax)]
        [JsonProperty(Names.Smoothness)]
        public float Smoothness { get; set; }
        
        [JsonProperty(Names.Rounded)]
        public bool Rounded { get; set; }

        public Vignette()
        {
            ColorHDR = ColorValue.black;
            Center = new Vector2Value(0.5f, 0.5f);
            Intensity = 0.3f;
            Smoothness = 0.5f;
            Rounded = false;
        }
        public Vignette(IColor colorHDR, IVector2 center, float intensity, float smoothness, bool rounded,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            ColorHDR = colorHDR;
            Center = center;
            Intensity = intensity;
            Smoothness = smoothness;
            Rounded = rounded;
        }

        public object Clone() => Copy();
        public Vignette Copy() => new(ColorHDR.Copy(), Center.Copy(), Intensity, Smoothness, Rounded, Frame, Ease);

        public override bool Equals(object obj) => obj is Vignette value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            ColorHDR, Center, Intensity, Smoothness, Rounded);

        public bool Equals(Vignette other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && ColorHDR.Equals(other.ColorHDR)
                         && Center.Equals(other.Center)
                         && Intensity.Equals(other.Intensity)
                         && Smoothness.Equals(other.Smoothness)
                         && Rounded == other.Rounded;
            return result;
        }
    }
}