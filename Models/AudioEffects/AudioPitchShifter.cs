using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioPitchShifter : AudioEffect
    {
        [RuleInRange(AudioRules.PitchShifter.Pitch_Min, AudioRules.PitchShifter.Pitch_Max)]
        [JsonProperty(Names.Pitch)]
        public float Pitch { get; set; }
        
        [RuleInRange(AudioRules.PitchShifter.FFTSize_Min, AudioRules.PitchShifter.FFTSize_Max)]
        [JsonProperty(Names.FFTSize)]
        public float FFTSize { get; set; }
        
        [RuleInRange(AudioRules.PitchShifter.Overlap_Min, AudioRules.PitchShifter.Overlap_Max)]
        [JsonProperty(Names.Overlap)]
        public float Overlap { get; set; }
        
        [RuleInRange(AudioRules.PitchShifter.MaxChannels_Min, AudioRules.PitchShifter.MaxChannels_Max)]
        [JsonProperty(Names.MaxChannels)]
        public float MaxChannels { get; set; }

        public AudioPitchShifter()
        {
            Pitch = AudioRules.PitchShifter.Pitch_Default;
            FFTSize = AudioRules.PitchShifter.FFTSize_Default;
            Overlap = AudioRules.PitchShifter.Overlap_Default;
            MaxChannels = AudioRules.PitchShifter.MaxChannels_Default;
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