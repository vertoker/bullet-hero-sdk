using BHSDK.Models.Interfaces.Values;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class GradientColorKeyValue
    {
        [JsonProperty("c")]
        public IColor ColorHDR { get; set; }
        
        [JsonProperty("t")]
        public IFloat Time { get; set; }
        
        public GradientColorKey Get()
        {
            var key = new GradientColorKey(ColorHDR.Get(), Time.Get());
            return key;
        }

        public GradientColorKeyValue()
        {
            ColorHDR = new ColorValue(Color.white);
            Time = new FloatValue(0f);
        }
        public GradientColorKeyValue(Color colorHDR, float time)
        {
            ColorHDR = new ColorValue(colorHDR);
            Time = new FloatValue(time);
        }
        public GradientColorKeyValue(IColor colorHDR, IFloat time)
        {
            ColorHDR = colorHDR;
            Time = time;
        }
        public GradientColorKeyValue(GradientColorKey key)
        {
            ColorHDR = new ColorValue(key.color);
            Time = new FloatValue(key.time);
        }
    }
}