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
            FadeInTime = AudioStatic.Normalize_FadeInTime;
            LowestVolume = AudioStatic.Normalize_LowestVolume;
            MaximumAmp = AudioStatic.Normalize_MaximumAmp;
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