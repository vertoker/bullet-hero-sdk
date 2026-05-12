using System;
using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.AudioEffects
{
    [RuleContainer]
    public class AudioPitchShifter : AudioEffect, ICopyable<AudioPitchShifter>, IEquatable<AudioPitchShifter>
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

        public new object Clone() => Copy();
        public new AudioPitchShifter Copy() => new(MixLevel, Pitch, FFTSize, Overlap, MaxChannels);

        public override bool Equals(object obj) => obj is AudioPitchShifter value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Pitch, FFTSize, Overlap, MaxChannels);

        public bool Equals(AudioPitchShifter other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Pitch.Equals(other.Pitch)
                         && FFTSize.Equals(other.FFTSize)
                         && Overlap.Equals(other.Overlap)
                         && MaxChannels.Equals(other.MaxChannels);
            return result;
        }
    }
}