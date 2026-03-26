using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioEffect
    {
        [JsonProperty(ModelNames.MixLevel)]
        public float MixLevel { get; set; }

        public AudioEffect()
        {
            MixLevel = -80f; // -80f - 0f, 0.1f, dB
        }
        public AudioEffect(float mixLevel)
        {
            MixLevel = mixLevel;
        }
    }
}