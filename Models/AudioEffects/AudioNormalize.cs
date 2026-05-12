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
    public class AudioNormalize : AudioEffect, ICopyable<AudioNormalize>, IEquatable<AudioNormalize>
    {
        [RuleInRange(AudioRules.Normalize.FadeInTime_Min, AudioRules.Normalize.FadeInTime_Max)]
        [JsonProperty(Names.FadeInTime)]
        public float FadeInTime { get; set; }
        
        [RuleInRange(AudioRules.Normalize.LowestVolume_Min, AudioRules.Normalize.LowestVolume_Max)]
        [JsonProperty(Names.LowestVolume)]
        public float LowestVolume { get; set; }
        
        [RuleInRange(AudioRules.Normalize.MaximumAmp_Min, AudioRules.Normalize.MaximumAmp_Max)]
        [JsonProperty(Names.MaximumAmp)]
        public float MaximumAmp { get; set; }

        public AudioNormalize()
        {
            FadeInTime = AudioRules.Normalize.FadeInTime_Default;
            LowestVolume = AudioRules.Normalize.LowestVolume_Default;
            MaximumAmp = AudioRules.Normalize.MaximumAmp_Default;
        }
        public AudioNormalize(float mixLevel, float fadeInTime,
            float lowestVolume, float maximumAmp) : base(mixLevel)
        {
            FadeInTime = fadeInTime;
            LowestVolume = lowestVolume;
            MaximumAmp = maximumAmp;
        }

        public new object Clone() => Copy();
        public new AudioNormalize Copy() => new(MixLevel, FadeInTime, LowestVolume, MaximumAmp);

        public override bool Equals(object obj) => obj is AudioNormalize value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), FadeInTime, LowestVolume, MaximumAmp);

        public bool Equals(AudioNormalize other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && FadeInTime.Equals(other.FadeInTime)
                         && LowestVolume.Equals(other.LowestVolume)
                         && MaximumAmp.Equals(other.MaximumAmp);
            return result;
        }
    }
}