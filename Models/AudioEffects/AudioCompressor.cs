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
    public class AudioCompressor : AudioEffect, ICopyable<AudioCompressor>, IEquatable<AudioCompressor>
    {
        [RuleInRange(AudioRules.Compressor.Threshold_Min, AudioRules.Compressor.Threshold_Max)]
        [JsonProperty(Names.Threshold)]
        public float Threshold { get; set; }
        
        [RuleInRange(AudioRules.Compressor.Attack_Min, AudioRules.Compressor.Attack_Max)]
        [JsonProperty(Names.Attack)]
        public float Attack { get; set; }
        
        [RuleInRange(AudioRules.Compressor.Release_Min, AudioRules.Compressor.Release_Max)]
        [JsonProperty(Names.Release)]
        public float Release { get; set; }
        
        [RuleInRange(AudioRules.Compressor.MakeUpGain_Min, AudioRules.Compressor.MakeUpGain_Max)]
        [JsonProperty(Names.MakeUpGain)]
        public float MakeUpGain { get; set; }

        public AudioCompressor()
        {
            Threshold = AudioRules.Compressor.Threshold_Default;
            Attack = AudioRules.Compressor.Attack_Default;
            Release = AudioRules.Compressor.Release_Default;
            MakeUpGain = AudioRules.Compressor.MakeUpGain_Default;
        }
        public AudioCompressor(float mixLevel, float threshold,
            float attack, float release, float makeUpGain) : base(mixLevel)
        {
            Threshold = threshold;
            Attack = attack;
            Release = release;
            MakeUpGain = makeUpGain;
        }

        public new object Clone() => Copy();
        public new AudioCompressor Copy() => new(MixLevel, Threshold, Attack, Release, MakeUpGain);

        public override bool Equals(object obj) => obj is AudioCompressor value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Threshold, Attack, Release, MakeUpGain);

        public bool Equals(AudioCompressor other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Threshold.Equals(other.Threshold)
                         && Attack.Equals(other.Attack)
                         && Release.Equals(other.Release)
                         && MakeUpGain.Equals(other.MakeUpGain);
            return result;
        }
    }
}