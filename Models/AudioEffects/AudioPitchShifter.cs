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
            Pitch = AudioStatic.PitchShifter_PitchDefault;
            FFTSize = AudioStatic.PitchShifter_FFTSizeDefault;
            Overlap = AudioStatic.PitchShifter_OverlapDefault;
            MaxChannels = AudioStatic.PitchShifter_MaxChannelsDefault;
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