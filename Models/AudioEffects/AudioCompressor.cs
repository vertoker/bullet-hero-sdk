using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioCompressor : AudioEffect
    {
        [JsonProperty(ModelNames.Threshold)]
        public float Threshold { get; set; }
        
        [JsonProperty(ModelNames.Attack)]
        public float Attack { get; set; }
        
        [JsonProperty(ModelNames.Release)]
        public float Release { get; set; }
        
        [JsonProperty(ModelNames.MakeUpGain)]
        public float MakeUpGain { get; set; }

        public AudioCompressor()
        {
            Threshold = 0f; // -60f - 0f, 0.1f, dB
            Attack = 50f; // 10f - 200f, 1f, ms
            Release = 50f; // 20f - 1000f, 1f, ms
            MakeUpGain = 0f; // 0f - 30f, 0.1f, dB
        }
        public AudioCompressor(float mixLevel, float threshold,
            float attack, float release, float makeUpGain) : base(mixLevel)
        {
            Threshold = threshold;
            Attack = attack;
            Release = release;
            MakeUpGain = makeUpGain;
        }
    }
}