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
    public class Vignette : Keyframe, IModel<Vignette>
    {
        [RuleNotNull(typeof(ColorValue))] // TODO add extra part for checking HDR part
        [JsonProperty(Names.Color)]
        public IColor Color { get; set; }
        
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
            Color = ColorValue.black;
            Center = new Vector2Value(0.5f, 0.5f);
            Intensity = 0.3f;
            Smoothness = 0.5f;
            Rounded = false;
        }
        public Vignette(IColor color, IVector2 center, float intensity, float smoothness, bool rounded,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            Color = color;
            Center = center;
            Intensity = intensity;
            Smoothness = smoothness;
            Rounded = rounded;
        }
        public override void Reset()
        {
            base.Reset();
            Color = ColorValue.black;
            Center = new Vector2Value(0.5f, 0.5f);
            Intensity = 0.3f;
            Smoothness = 0.5f;
            Rounded = false;
        }
        
        public override object Clone() => CopyImpl();
        public override Keyframe Copy() => CopyImpl();
        Vignette ICopyable<Vignette>.Copy() => CopyImpl();
        
        private Vignette CopyImpl() => new(Color.Copy(), Center.Copy(), Intensity, Smoothness, Rounded, Frame, Ease);
        
        public override bool Equals(object obj) => obj is Vignette value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(),
            Color, Center, Intensity, Smoothness, Rounded);

        public bool Equals(Vignette other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Color.Equals(other.Color)
                         && Center.Equals(other.Center)
                         && Intensity.Equals(other.Intensity)
                         && Smoothness.Equals(other.Smoothness)
                         && Rounded == other.Rounded;
            return result;
        }
    }
}