using BHSDK.Models.Enum;
using BHSDK.Models.Keyframes;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    public class ColorCurves : Keyframe
    {
        [JsonProperty(ModelNames.Hue + ModelNames.Vs + ModelNames.Hue)]
        public float HueVsHue { get; set; }
        
        [JsonProperty(ModelNames.Sat + ModelNames.Vs + ModelNames.Sat)]
        public float SatVsSat { get; set; }
        
        // TODO add more values, especially for curves

        public ColorCurves()
        {
            HueVsHue = 0.5f;
            SatVsSat = 0.5f;
        }
        public ColorCurves(int frame, EaseType ease, 
            float hueVsHue, float satVsSat)
            : base(frame, ease)
        {
            HueVsHue = hueVsHue;
            SatVsSat = satVsSat;
        }
    }
}