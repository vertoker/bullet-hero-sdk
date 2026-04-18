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
            Pitch = AudioStatic.PitchShifter_Pitch;
            FFTSize = AudioStatic.PitchShifter_FFTSize;
            Overlap = AudioStatic.PitchShifter_Overlap;
            MaxChannels = AudioStatic.PitchShifter_MaxChannels;
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