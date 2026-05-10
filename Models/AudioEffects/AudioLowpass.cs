using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioLowpass : AudioEffect
    {
        [RuleInRange(AudioRules.Lowpass.CutoffFreq_Min, AudioRules.Lowpass.CutoffFreq_Max)]
        [JsonProperty(Names.CutoffFreq)]
        public float CutoffFreq { get; set; }

        public AudioLowpass()
        {
            CutoffFreq = AudioRules.Lowpass.CutoffFreq_Default;
        }
        public AudioLowpass(float mixLevel, float cutoffFreq) : base(mixLevel)
        {
            CutoffFreq = cutoffFreq;
        }
    }
}