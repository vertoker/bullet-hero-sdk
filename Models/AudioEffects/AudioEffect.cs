using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    [RuleContainer]
    public class AudioEffect : ICopyable<AudioEffect>
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
    }
}