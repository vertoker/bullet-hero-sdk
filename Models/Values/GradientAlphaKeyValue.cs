using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class GradientAlphaKeyValue
    {
        [JsonProperty(ModelNames.Alpha)]
        public FloatValue Alpha { get; set; }
        
        [JsonProperty(ModelNames.Time)]
        public FloatValue Time { get; set; }

        public GradientAlphaKey Get()
        {
            var key = new GradientAlphaKey(Alpha.Get(), Time.Get());
            return key;
        }

        public GradientAlphaKeyValue()
        {
            Alpha = new FloatValue(1f);
            Time = new FloatValue(0f);
        }
        public GradientAlphaKeyValue(float alpha, float time)
        {
            Alpha = new FloatValue(alpha);
            Time = new FloatValue(time);
        }
        public GradientAlphaKeyValue(FloatValue alpha, FloatValue time)
        {
            Alpha = alpha;
            Time = time;
        }
        public GradientAlphaKeyValue(GradientAlphaKey key)
        {
            Alpha = new FloatValue(key.alpha);
            Time = new FloatValue(key.time);
        }
    }
}