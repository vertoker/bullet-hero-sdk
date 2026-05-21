using System;
using BH.SDK.Models.Interfaces;
using BH.SDK.Rules;
using BH.SDK.Rules.Attributes;
using Newtonsoft.Json;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BH.SDK.Models.AudioEffects
{
    [RuleContainer]
    public class AudioEffect : ICopyable<AudioEffect>, IEquatable<AudioEffect>
    {
        [RuleInRange(AudioRules.MixLevel_Disabled, AudioRules.MixLevel_Enabled)]
        [JsonProperty(Names.MixLevel)]
        public float MixLevel { get; set; }

        public AudioEffect()
        {
            MixLevel = AudioRules.MixLevel_Default;
        }
        public AudioEffect(float mixLevel)
        {
            MixLevel = mixLevel;
        }

        public object Clone() => Copy();
        public AudioEffect Copy() => new(MixLevel);

        public override bool Equals(object obj) => obj is AudioEffect value && Equals(value);
        public override int GetHashCode() => MixLevel.GetHashCode();

        public bool Equals(AudioEffect other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var result = MixLevel.Equals(other.MixLevel);
            return result;
        }
    }
}