using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioPitchShifter : AudioEffect
    {
        [JsonProperty(Names.Pitch)]
        public float Pitch { get; set; }
        
        [JsonProperty(Names.FFTSize)]
        public float FFTSize { get; set; }
        
        [JsonProperty(Names.Overlap)]
        public float Overlap { get; set; }
        
        [JsonProperty(Names.MaxChannels)]
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