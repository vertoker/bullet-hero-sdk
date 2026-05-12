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
    public class AudioHighpass : AudioEffect, ICopyable<AudioHighpass>, IEquatable<AudioHighpass>
    {
        [RuleInRange(AudioRules.Highpass.CutoffFreq_Min, AudioRules.Highpass.CutoffFreq_Max)]
        [JsonProperty(Names.CutoffFreq)]
        public float CutoffFreq { get; set; }

        public AudioHighpass()
        {
            CutoffFreq = AudioRules.Highpass.CutoffFreq_Default;
        }
        public AudioHighpass(float mixLevel, float cutoffFreq) : base(mixLevel)
        {
            CutoffFreq = cutoffFreq;
        }

        public new object Clone() => Copy();
        public new AudioHighpass Copy() => new(MixLevel, CutoffFreq);

        public override bool Equals(object obj) => obj is AudioHighpass value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), CutoffFreq);

        public bool Equals(AudioHighpass other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && CutoffFreq.Equals(other.CutoffFreq);
            return result;
        }
    }
}