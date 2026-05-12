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
    public class AudioParamEQ : AudioEffect, ICopyable<AudioParamEQ>, IEquatable<AudioParamEQ>
    {
        [RuleInRange(AudioRules.ParamEQ.CenterFreq_Min, AudioRules.ParamEQ.CenterFreq_Max)]
        [JsonProperty(Names.CenterFreq)]
        public float CenterFreq { get; set; }
        
        [RuleInRange(AudioRules.ParamEQ.OctaveRange_Min, AudioRules.ParamEQ.OctaveRange_Max)]
        [JsonProperty(Names.OctaveRange)]
        public float OctaveRange { get; set; }
        
        [RuleInRange(AudioRules.ParamEQ.FrequencyGain_Min, AudioRules.ParamEQ.FrequencyGain_Max)]
        [JsonProperty(Names.FreqGain)]
        public float FrequencyGain { get; set; }

        public AudioParamEQ()
        {
            CenterFreq = AudioRules.ParamEQ.CenterFreq_Default;
            OctaveRange = AudioRules.ParamEQ.OctaveRange_Default;
            FrequencyGain = AudioRules.ParamEQ.FrequencyGain_Default;
        }
        public AudioParamEQ(float mixLevel, float centerFreq,
            float octaveRange, float frequencyGain) : base(mixLevel)
        {
            CenterFreq = centerFreq;
            OctaveRange = octaveRange;
            FrequencyGain = frequencyGain;
        }

        public new object Clone() => Copy();
        public new AudioParamEQ Copy() => new(MixLevel, CenterFreq, OctaveRange, FrequencyGain);

        public override bool Equals(object obj) => obj is AudioParamEQ value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), CenterFreq, OctaveRange, FrequencyGain);

        public bool Equals(AudioParamEQ other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && CenterFreq.Equals(other.CenterFreq)
                         && OctaveRange.Equals(other.OctaveRange)
                         && FrequencyGain.Equals(other.FrequencyGain);
            return result;
        }
    }
}