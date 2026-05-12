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
    public class AudioDistortion : AudioEffect, ICopyable<AudioDistortion>, IEquatable<AudioDistortion>
    {
        [RuleInRange(AudioRules.Distortion.Level_Min, AudioRules.Distortion.Level_Max)]
        [JsonProperty(Names.Level)]
        public float Level { get; set; }

        public AudioDistortion()
        {
            Level = AudioRules.Distortion.Level_Default;
        }
        public AudioDistortion(float mixLevel, float level) : base(mixLevel)
        {
            Level = level;
        }

        public new object Clone() => Copy();
        public new AudioDistortion Copy() => new(MixLevel, Level);

        public override bool Equals(object obj) => obj is AudioDistortion value && Equals(value);
        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Level);

        public bool Equals(AudioDistortion other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = base.Equals(other)
                         && Level.Equals(other.Level);
            return result;
        }
    }
}