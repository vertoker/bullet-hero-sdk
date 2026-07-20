using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.AudioEffects
{
    [RuleContainer]
    public class AudioDistortion : AudioEffect, IModel<AudioDistortion>
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
        public override void Reset()
        {
            base.Reset();
            Level = AudioRules.Distortion.Level_Default;
        }

        public override object Clone() => CopyImpl();
        public override AudioEffect Copy() => CopyImpl();
        AudioDistortion ICopyable<AudioDistortion>.Copy() => CopyImpl();

        private AudioDistortion CopyImpl() => new(MixLevel, Level);

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