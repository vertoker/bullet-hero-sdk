using BHSDK.Models.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace BHSDK.Models.Values
{
    public class GradientAlphaKeyValue : ICopyable<GradientAlphaKeyValue>
    {
        // TODO maybe replace FloatValue to IFloat (in editor step)
        
        [JsonProperty(Names.AlphaShort)]
        public float Alpha { get; set; }
        
        [JsonProperty(Names.TimeShort)]
        public float Time { get; set; }

        public GradientAlphaKey Get()
        {
            var key = new GradientAlphaKey(Alpha, Time);
            return key;
        }

        public GradientAlphaKeyValue()
        {
            Alpha = 1f;
            Time = 0f;
        }
        public GradientAlphaKeyValue(float alpha, float time)
        {
            Alpha = alpha;
            Time = time;
        }
        public GradientAlphaKeyValue(GradientAlphaKey key)
        {
            Alpha = key.alpha;
            Time = key.time;
        }

        public GradientAlphaKeyValue Copy() => new(Alpha, Time);
    }
}