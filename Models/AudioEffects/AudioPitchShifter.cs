using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioPitchShifter : AudioEffect
    {
        [JsonProperty(ModelNames.Pitch)]
        public float Pitch { get; set; }
        
        [JsonProperty(ModelNames.FFTSize)]
        public float FFTSize { get; set; }
        
        [JsonProperty(ModelNames.Overlap)]
        public float Overlap { get; set; }
        
        [JsonProperty(ModelNames.MaxChannels)]
        public float MaxChannels { get; set; }

        public AudioPitchShifter()
        {
            Pitch = 1f; // 0.5f - 2f, 0.01f, x
            FFTSize = 1024f; // 256f - 4096f, 1f, -
            Overlap = 4f; // 1f - 32f, 0.1f, -
            MaxChannels = 0f; // 0f - 16f, 0.01f, ch
        }
        public AudioPitchShifter(float mixLevel, float pitch, float fftSize,
            float overlap, float maxChannels) : base(mixLevel)
        {
            Pitch = pitch;
            FFTSize = fftSize;
            Overlap = overlap;
            MaxChannels = maxChannels;
        }
    }
}