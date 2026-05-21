using System;
using BH.SDK.Models.Enum;
using BH.SDK.Models.Interfaces;
using BH.SDK.Models.Keyframes;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.PostProcessing
{
    [RuleContainer]
    public class ColorCurves : Keyframe,
        ICopyable<ColorCurves>, IEquatable<ColorCurves>
    {
        [RuleInRange(PostProcessingRules.ColorCurves.HueVsHueMin,
            PostProcessingRules.ColorCurves.HueVsHueMax)]
        [JsonProperty(Names.HueVsHue)]
        public float HueVsHue { get; set; }
        
        [RuleInRange(PostProcessingRules.ColorCurves.SatVsSatMin,
            PostProcessingRules.ColorCurves.SatVsSatMax)]
        [JsonProperty(Names.SatVsSat)]
        public float SatVsSat { get; set; }
        
        // TODO add more values, especially for curves

        public ColorCurves()
        {
            HueVsHue = 0.5f;
            SatVsSat = 0.5f;
        }
        public ColorCurves(float hueVsHue, float satVsSat,
            int frame, EaseType ease = DefaultEase) : base(frame, ease)
        {
            HueVsHue = hueVsHue;
            SatVsSat = satVsSat;
        }

        public object Clone() => Copy();
        public ColorCurves Copy() => new(HueVsHue, SatVsSat, Frame, Ease);

        public override bool Equals(object obj) => obj is ColorCurves value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), HueVsHue, SatVsSat);

        public bool Equals(ColorCurves other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && HueVsHue.Equals(other.HueVsHue)
                         && SatVsSat.Equals(other.SatVsSat);
            return result;
        }
    }
}