using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioDistortion : AudioEffect
    {
        [JsonProperty(ModelNames.Level)]
        public float Level { get; set; }

        public AudioDistortion()
        {
            Level = 0.5f;  // 0f - 1f, 0.01f, -
        }
        public AudioDistortion(float mixLevel, float level) : base(mixLevel)
        {
            Level = level;
        }
    }
}