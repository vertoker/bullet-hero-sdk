using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioEffect
    {
        [JsonProperty(ModelNames.MixLevel)]
        public float MixLevel { get; set; }

        public AudioEffect()
        {
            MixLevel = AudioStatic.MixLevelDefault;
        }
        public AudioEffect(float mixLevel)
        {
            MixLevel = mixLevel;
        }
    }
}