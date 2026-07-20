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
    public class ColorCurvesKey : PostProcessingKeyframe, IModel<ColorCurvesKey>
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

        public ColorCurvesKey()
        {
            HueVsHue = 0.5f;
            SatVsSat = 0.5f;
        }
        public ColorCurvesKey(float hueVsHue, float satVsSat,
            bool active, int frame, EaseType ease = Keyframe.DefaultEase) : base(active, frame, ease)
        {
            HueVsHue = hueVsHue;
            SatVsSat = satVsSat;
        }
        public override void Reset()
        {
            base.Reset();
            HueVsHue = 0.5f;
            SatVsSat = 0.5f;
        }
        
        public override object Clone() => CopyImpl();
        public override PostProcessingKeyframe Copy() => CopyImpl();
        ColorCurvesKey ICopyable<ColorCurvesKey>.Copy() => CopyImpl();
        
        private ColorCurvesKey CopyImpl() => new(HueVsHue, SatVsSat, Active, Frame, Ease);

        public override bool Equals(object obj) => obj is ColorCurvesKey value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), HueVsHue, SatVsSat);

        public bool Equals(ColorCurvesKey other)
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