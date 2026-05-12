using System;
using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BHSDK.Models.AudioEffects
{
    [RuleContainer]
    public class AudioLowpass : AudioEffect, ICopyable<AudioLowpass>, IEquatable<AudioLowpass>
    {
        [RuleInRange(AudioRules.Lowpass.CutoffFreq_Min, AudioRules.Lowpass.CutoffFreq_Max)]
        [JsonProperty(Names.CutoffFreq)]
        public float CutoffFreq { get; set; }

        public AudioLowpass()
        {
            CutoffFreq = AudioRules.Lowpass.CutoffFreq_Default;
        }
        public AudioLowpass(float mixLevel, float cutoffFreq) : base(mixLevel)
        {
            CutoffFreq = cutoffFreq;
        }

        public new object Clone() => Copy();
        public new AudioLowpass Copy() => new(MixLevel, CutoffFreq);

        public override bool Equals(object obj) => obj is AudioLowpass value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), CutoffFreq);

        public bool Equals(AudioLowpass other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && CutoffFreq.Equals(other.CutoffFreq);
            return result;
        }
    }
}