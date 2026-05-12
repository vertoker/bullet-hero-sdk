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