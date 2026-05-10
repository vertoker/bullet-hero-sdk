using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioHighpass : AudioEffect
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
    }
}