using BHSDK.Models.Enum;
using BHSDK.Models.Interfaces;
using BHSDK.Models.Keyframes;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    [RuleContainer]
    public class ColorCurves : Keyframe, ICopyable<ColorCurves>
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
    }
}