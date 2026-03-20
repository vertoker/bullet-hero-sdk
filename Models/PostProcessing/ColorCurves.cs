using BHSDK.Models.Base;
using BHSDK.Models.Enum;
using Newtonsoft.Json;

namespace BHSDK.Models.PostProcessing
{
    public class ColorCurves : Keyframe
    {
        [JsonProperty("hvsh")]
        public float HueVsHue { get; set; }
        
        [JsonProperty("svss")]
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