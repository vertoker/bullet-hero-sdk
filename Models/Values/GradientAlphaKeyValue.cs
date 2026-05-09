using BHSDK.Models.Interfaces;
using Newtonsoft.Json;

namespace BHSDK.Models.Values
{
    public class GradientAlphaKeyValue : ICopyable<GradientAlphaKeyValue>
    {
        [JsonProperty(Names.AlphaShort)]
        public float Alpha { get; set; }
        
        [JsonProperty(Names.TimeShort)]
        public float Time { get; set; }
        
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

        public GradientAlphaKeyValue Copy() => new(Alpha, Time);
    }
}