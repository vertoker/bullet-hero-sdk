using BHSDK.Rules;
using BHSDK.Rules.Attributes;
using BHSDK.Utils;
using Newtonsoft.Json;

namespace BHSDK.Models.AudioEffects
{
    public class AudioParamEQ : AudioEffect
    {
        [RuleInRange(AudioRules.ParamEQ.CenterFreq_Min, AudioRules.ParamEQ.CenterFreq_Max)]
        [JsonProperty(Names.CenterFreq)]
        public float CenterFreq { get; set; }
        
        [RuleInRange(AudioRules.ParamEQ.OctaveRange_Min, AudioRules.ParamEQ.OctaveRange_Max)]
        [JsonProperty(Names.OctaveRange)]
        public float OctaveRange { get; set; }
        
        [RuleInRange(AudioRules.ParamEQ.FrequencyGain_Min, AudioRules.ParamEQ.FrequencyGain_Max)]
        [JsonProperty(Names.FreqGain)]
        public float FrequencyGain { get; set; }

        public AudioParamEQ()
        {
            CenterFreq = AudioRules.ParamEQ.CenterFreq_Default;
            OctaveRange = AudioRules.ParamEQ.OctaveRange_Default;
            FrequencyGain = AudioRules.ParamEQ.FrequencyGain_Default;
        }
        public AudioParamEQ(float mixLevel, float centerFreq,
            float octaveRange, float frequencyGain) : base(mixLevel)
        {
            CenterFreq = centerFreq;
            OctaveRange = octaveRange;
            FrequencyGain = frequencyGain;
        }
    }
}