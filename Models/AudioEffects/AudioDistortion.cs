using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioDistortion : AudioEffect
    {
        [JsonProperty(ModelNames.Level)]
        public float Level { get; set; }

        public AudioDistortion()
        {
            Level = AudioStatic.Distortion_LevelDefault;
        }
        public AudioDistortion(float mixLevel, float level) : base(mixLevel)
        {
            Level = level;
        }
    }
}