using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioNormalize : AudioEffect
    {
        [JsonProperty(Names.FadeInTime)]
        public float FadeInTime { get; set; }
        
        [JsonProperty(Names.LowestVolume)]
        public float LowestVolume { get; set; }
        
        [JsonProperty(Names.MaximumAmp)]
        public float MaximumAmp { get; set; }

        public AudioNormalize()
        {
            FadeInTime = AudioStatic.Normalize_FadeInTimeDefault;
            LowestVolume = AudioStatic.Normalize_LowestVolumeDefault;
            MaximumAmp = AudioStatic.Normalize_MaximumAmpDefault;
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