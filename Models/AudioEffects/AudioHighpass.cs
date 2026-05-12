using BHSDK.Models.Interfaces;
using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    [RuleContainer]
    public class AudioHighpass : AudioEffect, ICopyable<AudioHighpass>
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
    }
}