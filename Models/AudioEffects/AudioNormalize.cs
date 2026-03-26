using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioNormalize : AudioEffect
    {
        [JsonProperty(ModelNames.FadeInTime)]
        public float FadeInTime { get; set; }
        
        [JsonProperty(ModelNames.LowestVolume)]
        public float LowestVolume { get; set; }
        
        [JsonProperty(ModelNames.MaximumAmp)]
        public float MaximumAmp { get; set; }

        public AudioNormalize()
        {
            FadeInTime = 5000f; // 0f - 20000f, 1f, ms
            LowestVolume = 0.1f; // 0f - 1f, 0.01f, -
            MaximumAmp = 20f; // 0f - 100000f, 1f, x
        }
        public AudioNormalize(float mixLevel, float fadeInTime,
            float lowestVolume, float maximumAmp) : base(mixLevel)
        {
            FadeInTime = fadeInTime;
            LowestVolume = lowestVolume;
            MaximumAmp = maximumAmp;
        }
    }
}